#ifndef TOO_YOUNG_INTERSECTIONS_INCLUDED
#define TOO_YOUNG_INTERSECTIONS_INCLUDED
#include "Packages/com.chichi.render-pipelines.too-young/ShaderLibrary/RayTracing/Ray.hlsl"

struct HitResult
{
    float frontSide;
    float backSide;
};

bool IntersectSphere(Ray ray, float4 sphere, out HitResult result)
{
    result = (HitResult)0;

    float3 oc = sphere.xyz - ray.originWS;
    float a = dot(ray.directionWS, ray.directionWS);
    float b = -2.0 * dot(ray.directionWS, oc);
    float c = dot(oc, oc) - sphere.w * sphere.w;
    float discriminant = b * b - 4 * a * c;
    return (discriminant >= 0);
}

#endif