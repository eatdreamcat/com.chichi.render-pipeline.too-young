//
// This file was automatically generated. Please don't edit by hand. Execute Editor command [ Edit > Rendering > Generate Shader Includes ] instead
//

#ifndef MATERIALDATA_CS_HLSL
#define MATERIALDATA_CS_HLSL



// Generated from UnityEngine.Rendering.TooYoung.MaterialProperties
// PackingRules = Exact
struct MaterialProperties
{
    float4 albedoColor; // x: r y: g z: b w: a 
    float roughness;
    float metallic;
};

//
// Accessors for UnityEngine.Rendering.TooYoung.MaterialProperties
//
float4 GetAlbedoColor(MaterialProperties value)
{
    return value.albedoColor;
}
float GetRoughness(MaterialProperties value)
{
    return value.roughness;
}
float GetMetallic(MaterialProperties value)
{
    return value.metallic;
}

#endif
