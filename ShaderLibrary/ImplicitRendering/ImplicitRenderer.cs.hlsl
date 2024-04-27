//
// This file was automatically generated. Please don't edit by hand. Execute Editor command [ Edit > Rendering > Generate Shader Includes ] instead
//

#ifndef IMPLICITRENDERER_CS_HLSL
#define IMPLICITRENDERER_CS_HLSL
//
// UnityEngine.Rendering.TooYoung.ImplicitPrimitive:  static fields
//
#define IMPLICITPRIMITIVE_NONE (0)
#define IMPLICITPRIMITIVE_SPHERE (1)
#define IMPLICITPRIMITIVE_CUBE (2)
#define IMPLICITPRIMITIVE_PLANE (3)

// Generated from UnityEngine.Rendering.TooYoung.ImplicitRenderer
// PackingRules = Exact
struct ImplicitRenderer
{
    int primitive;
    uint instanceId;
};

//
// Accessors for UnityEngine.Rendering.TooYoung.ImplicitRenderer
//
int GetPrimitive(ImplicitRenderer value)
{
    return value.primitive;
}
uint GetInstanceId(ImplicitRenderer value)
{
    return value.instanceId;
}

#endif
