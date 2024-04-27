using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Rendering.TooYoung
{
   
    public class Sphere : ImplicitRenderer
    {
        [SerializeField]
        private float m_Radius;

        public float Radius
        {
            get => m_Radius;
            set
            {
                m_Radius = Mathf.Max(0.0f, value);
            }
        }
       
    }
}

