using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Rendering.TooYoung
{

    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    public class ImplicitRenderer : MonoBehaviour
    {
        public static List<ImplicitRenderer> s_ImplicitRendererList = new();
        
        private void OnEnable()
        {
            s_ImplicitRendererList.Add(this);
            OnEnabled();
        }


        private void OnDisable()
        {
            s_ImplicitRendererList.Remove(this);
            OnDisabled();
        }

        protected virtual void OnEnabled()
        {
            
        }

        protected virtual void OnDisabled()
        {
           
        }

        public virtual ImplicitPrimitive GetPrimitiveType()
        {
            throw new System.NotImplementedException("Should override this method in inherit class");
        } 
    }
}
