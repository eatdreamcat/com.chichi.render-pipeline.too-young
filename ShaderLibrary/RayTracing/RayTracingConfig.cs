namespace UnityEngine.Rendering.TooYoung
{
    [GenerateHLSL]
    public enum SampleringMethod
    {
        Uniform,
        Importance,
        MultipImportance
    }

    [GenerateHLSL(needAccessors = false, generateCBuffer = true, constantRegister = (int)ConstantRegister.Global)]
    internal struct RayTracingData
    {
       
    }
}
