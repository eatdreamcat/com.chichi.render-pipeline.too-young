#if UNITY_EDITOR
using UnityEditorInternal;
#endif

namespace UnityEngine.Rendering.TooYoung
{
#if UNITY_EDITOR
    
    public class TYProjectSettingsReadOnlyBase : ScriptableObject
    {
        public const string filePath = "ProjectSettings/TYRPProjectSettings.asset";

        [SerializeField]
        protected string m_ProjectSettingFolderPath = "TYRPDefaultResources";

        public static string projectSettingsFolderPath => instance.m_ProjectSettingFolderPath;

        //singleton pattern
        protected static TYProjectSettingsReadOnlyBase s_Instance;
        static TYProjectSettingsReadOnlyBase instance => s_Instance ?? CreateOrLoad();

        protected TYProjectSettingsReadOnlyBase()
        {
            s_Instance = this;
        }

        static TYProjectSettingsReadOnlyBase CreateOrLoad()
        {
            //try load: if it exists, this will trigger the call to the private ctor
            InternalEditorUtility.LoadSerializedFileAndForget(filePath);

            //else create
            if (s_Instance == null)
            {
                TYProjectSettingsReadOnlyBase created = CreateInstance<TYProjectSettingsReadOnlyBase>();
                created.hideFlags = HideFlags.HideAndDontSave;
            }

            return s_Instance;
        }
    }
    
#endif
}
