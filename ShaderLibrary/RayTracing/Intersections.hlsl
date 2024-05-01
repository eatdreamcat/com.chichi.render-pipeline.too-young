#ifndef TOO_YOUNG_INTERSECTIONS_INCLUDED
#define TOO_YOUNG_INTERSECTIONS_INCLUDED
#include "Packages/com.chichi.render-pipelines.too-young/ShaderLibrary/RayTracing/Ray.hlsl"

struct HitResult
{
    float t0;
    float t1;
    float3 normalOutside;
    float3 normalInside;
};

bool IntersectSphere(Ray ray, float4 sphere, out HitResult result)
{
    result = (HitResult)0;

    float3 oc = sphere.xyz - ray.originWS;
    float a = dot(ray.directionWS, ray.directionWS);
    float b = -2.0 * dot(ray.directionWS, oc);
    float c = dot(oc, oc) - sphere.w * sphere.w;
    float discriminant = b * b - 4 * a * c;

    if (discriminant < 0)
    {
        return false;
    }

    result.t0 = (-b - sqrt(discriminant)) / (2.0 * a);
    result.t1 = (-b + sqrt(discriminant)) / (2.0 * a);

    // TODO: fix this bug 
    if (result.t0 < 0 && result.t1 < 0) return false;
    
    result.normalOutside = normalize(EvaluateRayPosition(ray, result.t0) - sphere.xyz);
    result.normalInside = normalize(sphere.xyz - EvaluateRayPosition(ray, result.t1));
    return true;
}

#endif
