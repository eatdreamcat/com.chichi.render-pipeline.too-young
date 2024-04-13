using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.TooYoung;

namespace UnityEditor.Rendering.TooYoung
{
    class DoCreateNewAssetTYRenderPipeline : ProjectWindowCallback.EndNameEditAction
    {
        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            var newAsset = CreateInstance<TYRenderPipelineAsset>();
            newAsset.name = Path.GetFileName(pathName);

            AssetDatabase.CreateAsset(newAsset, pathName);
            ProjectWindowUtil.ShowCreatedAsset(newAsset);
        }
    }

    static partial class TYAssetFactor
    {
        [MenuItem("Assets/Create/Rendering/TYRP Asset", 
            priority = CoreUtils.Sections.section1 + CoreUtils.Priorities.assetsCreateRenderingMenuPriority)]
        static void CreateHDRenderPipeline()
        {
            var icon = EditorGUIUtility.FindTexture("ScriptableObject Icon");
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, 
                ScriptableObject.CreateInstance<DoCreateNewAssetTYRenderPipeline>(), 
                "New TYRenderPipelineAsset.asset", icon, null);
        }
    }
}
