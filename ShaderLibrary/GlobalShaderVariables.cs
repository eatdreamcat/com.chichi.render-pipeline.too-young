namespace UnityEngine.Rendering.TooYoung
{
    [GenerateHLSL(needAccessors = false, generateCBuffer = true, constantRegister = (int)ConstantRegister.Global)]
    public struct GlobalShaderVariables
    {
        public Vector3 _CameraWorldPosition;
    }
}