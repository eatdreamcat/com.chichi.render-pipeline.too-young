#ifndef TOO_YOUNG_RAY_INCLUDED
#define TOO_YOUNG_RAY_INCLUDED
#include "Packages/com.chichi.render-pipeline.too-young/ShaderLibrary/RayTracing/RayTracingConfig.cs.hlsl"

struct Ray
{
    float3 originWS;
    float3 directionWS;
};

float3 EvaluateRayPosition(Ray ray, float t)
{
    return ray.originWS + ray.directionWS * t;
}



#endif