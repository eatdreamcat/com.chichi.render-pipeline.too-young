namespace UnityEngine.Rendering.TooYoung
{
    [GenerateHLSL(needAccessors = false, generateCBuffer = true, constantRegister = (int)ConstantRegister.Global)]
    public unsafe struct GlobalShaderVariables
    {
        // should be aligned to float4
        public Vector4 _CameraWorldPosition;
        [HLSLArray(ConstDefine.k_MAX_IMPLICIT_RENDERER_COUNT, typeof(Vector4))]
        public fixed float _ImplicitSphereList[ConstDefine.k_MAX_IMPLICIT_RENDERER_COUNT * ConstDefine.k_IMPLICIT_SPHERE_STRIDE];    }
}