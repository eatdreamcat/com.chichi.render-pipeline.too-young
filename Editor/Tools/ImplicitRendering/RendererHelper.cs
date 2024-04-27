using UnityEngine;
using UnityEngine.Rendering.TooYoung;

namespace UnityEditor.Rendering.TooYoung
{
    public static class RendererHelper
    {
        private static readonly string s_Sphere = "Sphere(Implicit)";
        [MenuItem("GameObject/3D Object/Implicit/Sphere")]
        static void CreateImplicitSphere(MenuCommand menuCommand)
        {
            DoCreateImplicitSphere(menuCommand.context);
        }

        static void DoCreateImplicitSphere(Object context)
        {
            var go = new GameObject();
            go.AddComponent<Sphere>();
            go.AddComponent<UberMaterial>();
            go.name = s_Sphere;
            if (context != null && context is GameObject)
            {
                go.transform.parent = (context as GameObject).transform;
            }
        }
    }
}