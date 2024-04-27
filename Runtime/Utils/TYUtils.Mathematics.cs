using System;
using UnityEngine;

namespace UnityEngine.Rendering.TooYoung
{
    public static partial class TYUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="renderTargetResolution">x:width, y:height, z:inverse width, w: inverse height</param>
        /// <returns></returns>
        internal static Matrix4x4 ComputePixelCoordToWorldSpaceViewDirectionMatrix(
            Camera camera, 
            Vector4 renderTargetResolution)
        {

            Matrix4x4 result = Matrix4x4.identity;
            if (camera.orthographic)
            {
                // TODO
            }
            else
            {
                var inverseView = camera.cameraToWorldMatrix;
                var halfFov = camera.fieldOfView * Mathf.Deg2Rad / 2.0f;
                var vertical = 2.0f * Mathf.Tan(halfFov) * camera.nearClipPlane;
                var horizontal = vertical * camera.aspect;
                
                var scaleAndOffsetMatrix = Matrix4x4.Scale(new Vector3(
                    horizontal * renderTargetResolution.z,
                    vertical * renderTargetResolution.w,
                    -camera.nearClipPlane
                    ));

                scaleAndOffsetMatrix.m03 = -horizontal / 2.0f;
                scaleAndOffsetMatrix.m13 = -vertical / 2.0f;
                if (SystemInfo.graphicsUVStartsAtTop)
                {
                    scaleAndOffsetMatrix.m11 *= -1;
                    scaleAndOffsetMatrix.m13 = vertical / 2.0f;
                }
                
                result = inverseView * scaleAndOffsetMatrix;
            }
            
            return result;
        }
        
    }
}