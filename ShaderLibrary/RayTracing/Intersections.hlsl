#ifndef TOO_YOUNG_INTERSECTIONS_INCLUDED
#define TOO_YOUNG_INTERSECTIONS_INCLUDED

#include "Packages/com.chichi.render-pipelines.too-young/ShaderLibrary/ImplicitRendering/ImplicitRenderer.cs.hlsl"
#include "Packages/com.chichi.render-pipelines.too-young/ShaderLibrary/RayTracing/Ray.hlsl"

struct HitResult
{
    float t;
    float3 normal;
    float3 position;
    float face;
};

bool IntersectSphere(Ray ray, float4 sphere, out HitResult result)
{
    result = (HitResult)0;

    float3 oc = sphere.xyz - ray.originWS;
    float a = dot(ray.directionWS, ray.directionWS);
    float h = dot(ray.directionWS, oc);
    float c = dot(oc, oc) - sphere.w * sphere.w;
    float discriminant = h * h - a * c;

    if (discriminant < 0)
    {
        return false;
    }
    
    result.t = (h - sqrt(discriminant)) * rcp(a);
    // TODO: 应该控制在near跟far之间
    if (result.t < 0)
    {
        return false;
    }

    result.position = EvaluateRayPosition(ray, result.t);
    result.normal = normalize(result.position - sphere.xyz);
    
    if (dot(ray.directionWS, result.normal) > 0)
    {
        result.normal = -result.normal;
        result.face = FACETYPE_BACK;
    }
    else
    {
        result.face = FACETYPE_FRONT;
        
    }
    
    return true;
}

#endif
