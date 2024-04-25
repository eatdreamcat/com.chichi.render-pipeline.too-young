using System;

namespace UnityEngine.Rendering.TooYoung
{
    [Serializable]
    [SupportedOnRenderPipeline(typeof(TYRenderPipelineAsset))]
    [Categorization.CategoryInfo(Name = "R: Runtime Materials", Order = 1000), HideInInspector]
    class TYRenderPipelineRuntimeMaterials : IRenderPipelineResources
    {
        public int version => 0;

        bool IRenderPipelineGraphicsSettings.isAvailableInPlayerBuild => true;
    }
}