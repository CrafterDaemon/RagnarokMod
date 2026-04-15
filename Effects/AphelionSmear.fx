// AphelionSmear.fx
// Radial zoom blur motion smear for the Aphelion scythe.
//
// Technique: resamples the sprite texture at progressively rotated UV
// coordinates around the sprite pivot, accumulating with a falloff weight
// so the leading edge is bright and the trail fades behind it.
//
// Based on the zoom blur pattern — instead of zooming outward from center,
// we rotate around center, which creates a rotational motion blur.
//
// ps_3_0: needed for the loop.
//
// Parameters:
//   blurAngle   — total arc to smear across (radians). Driven by angular
//                 velocity: faster spin = wider smear. Typically 0.05–0.4.
//   trailFade   — how dim the tail end is. 0.0 = invisible, 1.0 = full.
//   tintColor   — charge-tinted color multiplied into the result.

sampler2D Texture : register(s0);

float  blurAngle;
float  trailFade;
float4 tintColor;

#define SAMPLES 16
#define PIVOT   float2(0.5, 0.5)

float2 RotateAround(float2 uv, float angle)
{
    float2 d = uv - PIVOT;
    float  s = sin(angle);
    float  c = cos(angle);
    return float2(d.x * c - d.y * s, d.x * s + d.y * c) + PIVOT;
}

float4 PixelShaderFunction(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 result = 0;

    for (int i = 0; i < SAMPLES; i++)
    {
        // t = 0 is the leading edge (no rotation offset = current position).
        // t = 1 is the tail end (full blurAngle behind).
        float t = (float)i / (float)(SAMPLES - 1);

        // Rotate backwards from current position into the trail.
        float angle = -blurAngle * t;
        float2 uv   = RotateAround(coords, angle);

        // Discard out-of-bounds samples cleanly.
        float inBounds = step(0.0, uv.x) * step(uv.x, 1.0)
                       * step(0.0, uv.y) * step(uv.y, 1.0);

        // Weight: leading edge full brightness, tail fades by trailFade curve.
        float weight = lerp(1.0, trailFade, pow(t, 1.5));

        result += tex2D(Texture, uv) * weight * inBounds;
    }

    // Normalise so total brightness stays consistent regardless of sample count.
    // We don't divide fully (would make it too dim) — divide by half SAMPLES
    // so the result is brighter than average, giving the glowy additive feel.
    result /= (SAMPLES * 0.5);

    return result * tintColor * sampleColor;
}

technique AphelionSmear
{
    pass AutoloadPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
