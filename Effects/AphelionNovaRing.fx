// AphelionNovaRing.fx
// Expanding shockwave ring for the Aphelion supernova.
//
// Draws a thin annulus on a square quad. The ring is defined by two radii:
// innerRadius and outerRadius (both in normalised 0-1 UV space).
// A noise texture modulates the ring edge to break up the perfect circle.
//
// s0: noise texture bound by SpriteBatch
//
// Parameters:
//   progress        — 0→1 expansion progress
//   opacity         — overall alpha, used for fade-in and fade-out
//   ringColor       — RGB tint of the ring
//   noiseStrength   — how much the noise distorts the ring edge (0.0–0.08)
//   spinTime        — slowly rotates the noise so the ring shimmers

sampler2D noiseMap : register(s0);

float progress;
float opacity;
float noiseStrength;
float spinTime;
float3 ringColor;

struct PSInput
{
    float4 sampleColor : COLOR0;
    float2 coords : TEXCOORD0;
};

float4 PixelShaderFunction(PSInput input) : COLOR0
{
    float2 centred = input.coords * 2.0 - 1.0;
    float dist = length(centred);

    // Sample noise angularly around the ring to roughen the edges.
    float angle = atan2(centred.y, centred.x);
    float2 noiseUV = float2(angle / 6.2832 + spinTime * 0.02, progress * 0.5 + 0.25);
    float noise = tex2D(noiseMap, noiseUV).r * 2.0 - 1.0; // -1 to 1

    float distorted = dist + noise * noiseStrength;

    // Ring band: progress drives expansion. Ring starts at 0 and grows to 1.
    // Inner edge is sharp, outer edge is soft.
    float ringCenter = progress;
    
    float ringWidth = 0.12 + progress * 0.08; // widens more aggressively
    float innerEdge = ringCenter - ringWidth * 0.25;
    float outerEdge = ringCenter + ringWidth * 0.75; // asymmetric: long soft tail outward

    // Sharp inner edge, soft outer falloff.
    float innerMask = smoothstep(innerEdge - 0.01, innerEdge + 0.02, distorted);
    float outerMask = 1.0 - smoothstep(outerEdge - 0.03, outerEdge + 0.02, distorted);
    float ringMask = innerMask * outerMask;

    // Brightness: peaks at the inner edge (hottest part of a shockwave).
    // Remap so inner edge = 1.0, outer edge = 0.0, then apply power curve.
    float brightness = 1.0 - smoothstep(innerEdge, outerEdge, distorted);
    brightness = pow(brightness, 1.8); // sharper peak at inner edge

    // Secondary inner glow — a softer ring just inside the main ring.
    float innerGlow = smoothstep(innerEdge - 0.06, innerEdge - 0.01, distorted)
                     * (1.0 - smoothstep(innerEdge - 0.01, innerEdge + 0.01, distorted));
    innerGlow *= 0.5;

    // Boost center brightness and add a white-hot core at the inner edge.
    float hotspot = pow(saturate(1.0 - (distorted - innerEdge) / (ringWidth * 0.3)), 3.0);
    float3 finalColor = ringColor * (brightness * 2.5 + innerGlow)
                  + float3(1.0, 0.95, 0.8) * hotspot * 1.8;
    float finalAlpha = (ringMask + innerGlow * 0.5) * opacity * input.sampleColor.a;

    // Hard clip outside radius 1 — the quad corners shouldn't show anything.
    finalAlpha *= step(dist, 1.05);

    return float4(finalColor, finalAlpha);
}

technique AphelionNovaRing
{
    pass AutoloadPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
