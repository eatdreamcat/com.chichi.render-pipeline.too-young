using System;

namespace UnityEngine.Rendering.TooYoung
{
    [Serializable]
    [SupportedOnRenderPipeline(typeof(TYRenderPipelineAsset))]
    [Categorization.CategoryInfo(Name = "R: Runtime Shaders", Order = 1000), HideInInspector]
    class TYRenderPipelineRuntimeShaders : IRenderPipelineResources
    {
        public int version => 0;

        bool IRenderPipelineGraphicsSettings.isAvailableInPlayerBuild => true;

        #region RayTracing Series

        [Header("RayTracingInOneWeekend")]
        [SerializeField, ResourcePath("Shaders/RayTracingSeries/RayTracingInOneWeekend.compute")]
        private ComputeShader m_RayTracingInOneWeekendCS;

        public ComputeShader rayTracingInOneWeekendCS
        {
            get => m_RayTracingInOneWeekendCS;
            set => this.SetValueAndNotify(ref m_RayTracingInOneWeekendCS, value);
        }

        #endregion

        #region Blitter

        [SerializeField, ResourcePath("ShaderLibrary/Blitter/Blit.shader")]
        private Shader m_BlitPS;

        public Shader blitPS
        {
            get => m_BlitPS;
            set => this.SetValueAndNotify(ref m_BlitPS, value);
        }

        [SerializeField, ResourcePath("ShaderLibrary/Blitter/BlitColorAndDepth.shader")]
        private Shader m_BlitColorAndDepthPS;

        public Shader blitColorAndDepthPS
        {
            get => m_BlitColorAndDepthPS;
            set => this.SetValueAndNotify(ref m_BlitColorAndDepthPS, value);
        }
        

        #endregion
    }
}