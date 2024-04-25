using System;

namespace UnityEngine.Rendering.TooYoung
{
    [Serializable]
    [SupportedOnRenderPipeline(typeof(TYRenderPipelineAsset))]
    [Categorization.CategoryInfo(Name = "R: Runtime Textures", Order = 1000), HideInInspector]
    class TYRenderPipelineRuntimeTextures : IRenderPipelineResources
    {
        public int version => 0;

        bool IRenderPipelineGraphicsSettings.isAvailableInPlayerBuild => true;
    }
}