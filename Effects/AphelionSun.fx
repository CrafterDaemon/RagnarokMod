// AphelionSun.fx
// Procedural stellar surface shader for the Aphelion scythe's orbiting suns.
sampler2D primaryNoise   : register(s0);
sampler2D secondaryNoise : register(s1);
sampler2D flowNoise      : register(s2);

float  globalTime;
float  spinTime;
float  coronaIntensityFactor;
float3 mainColor;
float3 darkerColor;
float3 subtractiveAccentFactor;

float Remap01(float from, float to, float x)
{
    return saturate((x - from) / (to - from));
}

// Front-face sphere projection.
// Treats the disc as the visible face of a 3D sphere and computes
// the spherical (longitude, latitude) UV of each surface point.
// spinOffset rotates the sphere around its Y axis (east-west spin),
// which is what a real rotating star does.
float2 SphereProject(float2 disc, float spinOffset)
{
    float r = length(disc);
    // disc.xy are the X and Y coordinates on the sphere face.
    // Z is reconstructed from the unit-sphere constraint: x²+y²+z²=1.
    // Clamp r so we only project inside the disc (r < 1).
    float rClamped = min(r, 0.999);
    float z = sqrt(1.0 - rClamped * rClamped);

    // 3D point on sphere surface visible from the front (+Z axis).
    float3 p = float3(disc.x, disc.y, z);

    // Rotate around Y axis by spinOffset (longitude spin).
    float angle = spinOffset * 6.2832;
    float cosA = cos(angle);
    float sinA = sin(angle);
    float3 rotated = float3(
        p.x * cosA + p.z * sinA,
        p.y,
       -p.x * sinA + p.z * cosA
    );

    // Spherical UV: longitude from atan2, latitude from asin.
    float u = atan2(rotated.x, rotated.z) / 6.2832 + 0.5;
    float v = asin(clamp(rotated.y, -1.0, 1.0)) / 3.1416 + 0.5;
    return float2(u, v);
}

// Turbulence: abs(noise - 0.5) * 2, then sample again at displaced coords.
// Creates sharper, more chaotic surface detail than plain noise.
float Turbulence(float2 uv, float2 flowOffset)
{
    float base = tex2D(primaryNoise, uv).r;
    float sharp = abs(base - 0.5) * 2.0;
    float2 displaced = uv + float2(sharp * 0.08, 0) + flowOffset;
    return tex2D(secondaryNoise, displaced).r;
}

// Gaussian corona band peaks at a target radius, falls off smoothly.
// Much smoother than a division singularity and easier to tune.
float CoronaBand(float dist, float peakDist, float width)
{
    float x = (dist - peakDist) / width;
    return exp(-x * x * 4.0);
}


float4 PixelShaderFunction(float4 sampleColor : COLOR0,
                            float2 coords      : TEXCOORD0) : COLOR0
{
    float2 centred   = coords * 2.0 - 1.0;
    float  radiusSqr = dot(centred, centred);
    float  radius    = sqrt(radiusSqr);

    // Soft circular fade, disc visible within radius ~0.72, fades by 0.82.
    // Natural fade, no hard clip.
    float discMask = Remap01(0.82, 0.68, radius);
    
    float2 sphereUV = SphereProject(centred * 0.72, spinTime * 0.08);
    
    // Sample a slow-moving flow field to animate the surface over time.
    float2 flowUV    = coords + float2(0.0, globalTime * 0.15);
    float2 flowShift = tex2D(flowNoise, flowUV * 0.5).rg * 0.06;
    
    // Two turbulence layers at different scales + speeds.
    float turbA = Turbulence(sphereUV + float2(globalTime * 0.04, 0), flowShift);
    float turbB = Turbulence(sphereUV * 1.8 + float2(0, globalTime * 0.06), flowShift * 0.5);

    // Combine: A is the broad structure, B adds fine granulation.
    float brightness = turbA * 0.65 + turbB * 0.35;
    
    // Regions where brightness dips create sunspot-like dark areas.
    float darkMask = Remap01(0.55, 0.38, brightness);
    
    float3 surfaceCol = lerp(darkerColor, mainColor, saturate(brightness * 1.5));

    // Subtractive accent darkens using the mask and the accent factor.
    // Using (1 - subtractiveAccentFactor) as darkening weight, same intent
    // as the original but driven purely by turbulence rather than a third sample.
    float accentWeight = (1.0 - subtractiveAccentFactor.r) * darkMask * 0.75;
    surfaceCol = lerp(surfaceCol, darkerColor * 0.3, accentWeight);

    // Hot veins — sharp bright lines from the turbulence peaks.
    float veins = pow(saturate(turbA * 1.3 - 0.3), 3.0) * 2.2;
    surfaceCol += mainColor * veins;
    
    // Physically motivated: brightness ~ cos(angle from centre) = sqrt(1-r^2).
    // Gives a strong spherical read without extra texture samples.
    float limb = sqrt(max(0.0, 1.0 - radiusSqr * 0.85));
    surfaceCol *= limb * 0.5 + 0.5;
    
    // Gaussian band centred at radius 0.75, width 0.18.
    // Modulate angularly with turbulence to make it irregular.
    float angle        = atan2(centred.y, centred.x);
    float2 coronaUV    = float2(angle / 6.2832 + spinTime * 0.03, 0.5);
    float coronaAngular = tex2D(primaryNoise, coronaUV).r * 0.6 + 0.4;

    float coronaBand    = CoronaBand(radius, 0.75, 0.18)
                        * coronaAngular
                        * coronaIntensityFactor;

    // Soft outer halo, very wide gaussian, purely additive bloom.
    float halo = CoronaBand(radius, 0.60, 0.45) * coronaIntensityFactor * 0.35;
    
    float3 coronaCol  = mainColor * 1.8;
    float3 finalColor = surfaceCol * discMask
                      + coronaCol  * coronaBand
                      + mainColor  * halo;

    // Alpha: disc + corona together, spawnProgress via sampleColor.a.
    float finalAlpha = saturate(discMask + coronaBand * 0.8 + halo)
                     * sampleColor.a;

    return float4(finalColor, finalAlpha);
}

technique AphelionSun
{
    pass AutoloadPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
