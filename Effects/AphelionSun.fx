sampler fireNoiseTexture     : register(s0);
sampler accentNoiseTexture   : register(s1);
sampler uvOffsetNoiseTexture : register(s2);

float  globalTime;
float  coronaIntensityFactor;
float  sphereSpinTime;
float  spawnProgress;
float3 mainColor;
float3 darkerColor;
float3 subtractiveAccentFactor;

float InverseLerp(float from, float to, float x)
{
    return saturate((x - from) / (to - from));
}

float4 PixelShaderFunction(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float2 coordsNormalizedToCenter = coords * 2 - 1;
    float distanceFromCenterSqr = dot(coordsNormalizedToCenter, coordsNormalizedToCenter) * 2;
    float starOpacity = InverseLerp(0.5, 0.42, distanceFromCenterSqr);
    
    float spherePinchFactor = (1 - sqrt(abs(1 - distanceFromCenterSqr))) / distanceFromCenterSqr + 0.045;
    float2 sphereCoords = coords * spherePinchFactor + float2(sphereSpinTime, 0);
    
    float starCoordsOffset = tex2D(fireNoiseTexture, sphereCoords).r * 0.41 + globalTime * 0.3;
    float2 starCoords = sphereCoords + float2(starCoordsOffset, 0);
    float3 starBrightnessTexture = tex2D(fireNoiseTexture, starCoords);
    
    float starGlow = saturate(1 - distanceFromCenterSqr * 0.91);
    
    float3 result = spherePinchFactor * mainColor * 0.777 + starGlow * darkerColor + starBrightnessTexture;
    
    result = lerp(result, darkerColor, saturate(1 - starBrightnessTexture.r) * 0.8);
    result -= (1 - subtractiveAccentFactor) * tex2D(accentNoiseTexture, sphereCoords * 2).r * 1.1;
    
    float2 uvOffset = tex2D(uvOffsetNoiseTexture, coords + float2(0, globalTime * 0.4));
    result += pow(tex2D(accentNoiseTexture, sphereCoords * 1.2 + uvOffset * 0.04).r, 2) * 2.1;
    
    float coronaFadeOut = InverseLerp(0.2, 0.5, distanceFromCenterSqr)
                        * InverseLerp(1.91, 0.98, distanceFromCenterSqr)
                        * coronaIntensityFactor;
    float coronaBrightness = coronaFadeOut / abs(distanceFromCenterSqr - 0.5 + uvOffset.y * 0.04 + 0.04);
    
    return (starOpacity * float4(result, 1) + float4(mainColor, 1) * coronaBrightness) * sampleColor;
}

technique AphelionSun
{
    pass AutoloadPass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
