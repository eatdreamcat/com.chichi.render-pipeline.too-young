#ifndef TOO_YOUNG_SHADER_VARIABLES_INCLUDED
#define TOO_YOUNG_SHADER_VARIABLES_INCLUDED

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GlobalSamplers.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureXR.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Packing.hlsl"
#include "Packages/com.chichi.render-pipelines.too-young/ShaderLibrary/ImplicitRendering/ImplicitRenderer.cs.hlsl"

StructuredBuffer<ImplicitRendererInfo> _ImplicitRendererList;

#endif