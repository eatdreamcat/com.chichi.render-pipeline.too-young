namespace UnityEngine.Rendering.TooYoung
{
    
    public static class ShaderIDs
    {
        #region Global Constant Buffer

        public static readonly int _GlobalShaderVariables = Shader.PropertyToID("GlobalShaderVariables");

        #endregion
        
        #region ImplicitRendering

        public static readonly int _PixelCoordToWorldMatrix = Shader.PropertyToID("_PixelCoordToWorldMatrix");
        public static readonly int _ClearColor = Shader.PropertyToID("_ClearColor");
        public static readonly int _ImplicitRendererList = Shader.PropertyToID("_ImplicitRendererList");
        public static readonly int _ImplicitRenderersCount = Shader.PropertyToID("_ImplicitRenderersCount");

        #endregion
    }
}