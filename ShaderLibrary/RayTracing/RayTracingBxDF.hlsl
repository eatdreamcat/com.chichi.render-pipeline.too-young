#ifndef TOO_YOUNG_RAYTRACINGBXDF_INCLUDED
#define TOO_YOUNG_RAYTRACINGBXDF_INCLUDED

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Random.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Sampling/Sampling.hlsl"
#include "Packages/com.chichi.render-pipelines.too-young/ShaderLibrary/RayTracing/Ray.hlsl"
#include "Packages/com.chichi.render-pipelines.too-young/ShaderLibrary/RayTracing/Intersections.hlsl"
#include "Packages/com.chichi.render-pipelines.too-young/ShaderLibrary/Noise.hlsl"

Ray SingleScattering(HitResult result, inout float3 finalColor)
{
    float2 U = float2(Float2ToRandomFloat(result.normal.xy), Float2ToRandomFloat(result.normal.zy));
    //(right-handed, Z up) coordinate.
    float3 randomDirRightHandedZUp = SampleHemisphereUniform(U.x, U.y);
    Ray ray = (Ray)0;
    ray.originWS = result.position + result.normal * 0.0001;
    ray.directionWS = SampleHemisphereCosine(U.x, U.y, result.normal);
    finalColor *= 0.5;
    
    return ray;
}


#endif