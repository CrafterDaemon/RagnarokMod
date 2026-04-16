// AphelionNovaCore.fx
// ps_3_0

sampler2D noiseMap : register(s0);

float globalTime;
float spinTime;
float instability;
float remnantFade;
float coreScale;
float3 mainColor;
float3 darkerColor;

float4 PixelShaderFunction(float4 sampleColor : COLOR0,
                            float2 coords : TEXCOORD0) : COLOR0
{
    float2 centred = coords * 2.0 - 1.0;
    float radiusSqr = dot(centred, centred);
    float scaledRSqr = radiusSqr / max(coreScale * coreScale, 0.001);
    float discMask = saturate((0.85 - radiusSqr) / 0.2);

// Zoom in much tighter on the texture, and use a proper spin rotation
// rather than a diagonal offset which produces streaking, not spinning.
    float cosA = cos(spinTime * 0.08);
    float sinA = sin(spinTime * 0.08);
    float2 rotated = float2(centred.x * cosA - centred.y * sinA,
                        centred.x * sinA + centred.y * cosA);

    float2 sphereUV = rotated * 0.18 + float2(globalTime * 0.04, globalTime * 0.025);

    float sA = tex2D(noiseMap, sphereUV).r;
    float sB = tex2D(noiseMap, sphereUV * 2.0 + float2(instability * 0.3, 0.0)).r;
    float surf = sA * 0.65 + sB * 0.35;

    float3 surfCol = lerp(darkerColor, mainColor, saturate(surf * 1.5));

    float vb = saturate(surf * 1.4 - 0.2);
    surfCol += mainColor * vb * vb * vb * (1.0 + instability * 4.0);

    float limb = saturate(1.0 - scaledRSqr);
    float limbW = lerp(0.5, 0.05, instability);
    surfCol *= limb * limbW + (1.0 - limbW);

    float whiteout = instability * instability;
    surfCol = lerp(surfCol, float3(2.2, 2.0, 1.8), whiteout);

    surfCol *= remnantFade;

    float flare = saturate(1.0 - scaledRSqr * 0.72)
                    * instability * 1.4 * remnantFade;
    float3 flareCol = mainColor * 2.5 * flare;

    float3 finalCol = surfCol * discMask + flareCol;
    float finalAlpha = saturate(discMask + flare * 0.5)
                     * remnantFade * sampleColor.a;

    return float4(finalCol, finalAlpha);
}

technique AphelionNovaCore
{
    pass AutoloadPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
