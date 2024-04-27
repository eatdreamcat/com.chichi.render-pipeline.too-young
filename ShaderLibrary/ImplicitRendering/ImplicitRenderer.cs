namespace UnityEngine.Rendering.TooYoung
{
    [GenerateHLSL]
    public enum ImplicitPrimitive
    {
        None = 0,
        Sphere,
        Cube,
        Plane
    }

    [GenerateHLSL]
    public unsafe struct ImplicitRendererInfo
    {
        public int primitive;
        public int instanceId;
    }
    
    [GenerateHLSL]
    public static class ConstDefine
    {
        public const int k_MAX_IMPLICIT_RENDERER_COUNT = 256;
        // match GlobalShaderVariables [HLSLArray(ConstDefine.k_MAX_IMPLICIT_RENDERER_COUNT, typeof(Vector4))]
        // xyz: center, w: radius
        // 4 float values for Vector4
        public const int k_IMPLICIT_SPHERE_STRIDE = 4;

    }
    
}

