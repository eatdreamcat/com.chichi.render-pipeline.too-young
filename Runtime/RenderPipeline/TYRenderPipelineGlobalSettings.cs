using System.Collections.Generic;
using System.ComponentModel;

namespace UnityEngine.Rendering.TooYoung
{
    [DisplayInfo(name = "TYRP Global Settings Asset", order = CoreUtils.Sections.section4 + 2)]
    [SupportedOnRenderPipeline(typeof(TYRenderPipelineAsset))]
    [DisplayName("TYRP")]
    partial class
        TYRenderPipelineGlobalSettings : RenderPipelineGlobalSettings<TYRenderPipelineGlobalSettings, TYRenderPipeline>
    {
        [SerializeField] RenderPipelineGraphicsSettingsContainer m_Settings = new();
        protected override List<IRenderPipelineGraphicsSettings> settingsList => m_Settings.settingsList;
            
#if UNITY_EDITOR
        internal static string defaultPath =>
            $"Assets/{TYProjectSettingsReadOnlyBase.projectSettingsFolderPath}/TYRenderPipelineGlobalSettings.asset";
        
        internal static TYRenderPipelineGlobalSettings Ensure(bool canCreateNewAsset = true)
        {
            TYRenderPipelineGlobalSettings currentInstance = GraphicsSettings.
                GetSettingsForRenderPipeline<TYRenderPipeline>() as TYRenderPipelineGlobalSettings;
            
            if (RenderPipelineGlobalSettingsUtils.TryEnsure<TYRenderPipelineGlobalSettings, TYRenderPipeline>(ref currentInstance, defaultPath, canCreateNewAsset))
            {
                return currentInstance;
            }

            return null;
        }

        public override void Initialize(RenderPipelineGlobalSettings source = null)
        {
            SetUpRPAssetIncluded();
            
        }

        void SetUpRPAssetIncluded()
        {
            if (!TryGet(typeof(IncludeAdditionalRPAssets), 
                    out var rpgs) || rpgs is not IncludeAdditionalRPAssets includer)
            {
                Debug.Log($"Missing {nameof(IncludeAdditionalRPAssets)} set up for TYRP.");
                return;
            }

            includer.includeReferencedInScenes = true;
            includer.includeAssetsByLabel = true;
            includer.labelToInclude = TYUtils.k_TyrpAssetBuildLabel;
        }
#endif
    }
}
