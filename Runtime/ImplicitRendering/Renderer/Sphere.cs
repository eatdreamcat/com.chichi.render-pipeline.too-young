using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Rendering.TooYoung
{
   
    // TODO: need custom inspector drawer
    public class Sphere : ImplicitRenderer
    {
        public static List<Sphere> s_ImplicitSpheres = new();
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

        public Vector3 Center
        {
            get
            {
                return new Vector3(transform.position.x, transform.position.y, transform.position.z);
            }
        }

        public override ImplicitPrimitive GetPrimitiveType()
        {
            return ImplicitPrimitive.Sphere;
        }

        protected override void OnDisabled()
        {
            s_ImplicitSpheres.Remove(this);
        }

        protected override void OnEnabled()
        {
            s_ImplicitSpheres.Add(this);
        }
    }
}

