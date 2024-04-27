namespace UnityEngine.Rendering.TooYoung
{
    [GenerateHLSL]
    public enum ImplicitPrimitive
    {
        None,
        Sphere,
        Cube,
        Plane
    }

    [GenerateHLSL]
    public struct ImplicitRenderer
    {
        public ImplicitPrimitive primitive;
        public uint instanceId;
    }

    [GenerateHLSL]
    public static class ConstDefine
    {
        const int k_MAX_RENDERER_COUNT = 1024;
    }
}

