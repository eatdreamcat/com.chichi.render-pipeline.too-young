//
// This file was automatically generated. Please don't edit by hand. Execute Editor command [ Edit > Rendering > Generate Shader Includes ] instead
//

#ifndef IMPLICITRENDERER_CS_HLSL
#define IMPLICITRENDERER_CS_HLSL
//
// UnityEngine.Rendering.TooYoung.FaceType:  static fields
//
#define FACETYPE_FRONT (0)
#define FACETYPE_BACK (1)

//
// UnityEngine.Rendering.TooYoung.ImplicitPrimitive:  static fields
//
#define IMPLICITPRIMITIVE_NONE (0)
#define IMPLICITPRIMITIVE_SPHERE (1)
#define IMPLICITPRIMITIVE_CUBE (2)
#define IMPLICITPRIMITIVE_PLANE (3)

//
// UnityEngine.Rendering.TooYoung.ConstDefine:  static fields
//
#define MAX_IMPLICIT_RENDERER_COUNT (256)
#define IMPLICIT_SPHERE_STRIDE (4)

// Generated from UnityEngine.Rendering.TooYoung.ImplicitRendererInfo
// PackingRules = Exact
struct ImplicitRendererInfo
{
    int primitive;
    int instanceId;
};

//
// Accessors for UnityEngine.Rendering.TooYoung.ImplicitRendererInfo
//
int GetPrimitive(ImplicitRendererInfo value)
{
    return value.primitive;
}
int GetInstanceId(ImplicitRendererInfo value)
{
    return value.instanceId;
}

#endif
