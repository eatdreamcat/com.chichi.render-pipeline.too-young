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
                // TODO: 
                var viewToWorldMatrix = camera.cameraToWorldMatrix;
                // camera coordinate is right-handed while we need left-handed here
                viewToWorldMatrix.SetColumn(2, viewToWorldMatrix.GetColumn(2) * -1);
                viewToWorldMatrix.SetColumn(3, new Vector4(0, 0, 0, 1));
                
                var halfFov = camera.fieldOfView * Mathf.Deg2Rad / 2.0f;
                var vertical = 2.0f * Mathf.Tan(halfFov) * camera.nearClipPlane;
                var horizontal = vertical * camera.aspect;
             
                var scaleAndOffsetMatrix = Matrix4x4.Scale(new Vector3(
                    horizontal * renderTargetResolution.z,  // [0, horizontal]
                    vertical * renderTargetResolution.w,    // [0, vertical]
                    camera.nearClipPlane                   
                    ));
              
                scaleAndOffsetMatrix.m03 = -horizontal / 2.0f;  // [-horizontal / 2, horizontal / 2]
                scaleAndOffsetMatrix.m13 = -vertical / 2.0f;    // [-vertical / 2, vertical / 2]
              
                // TODO： 这里很奇怪，感觉GameView跟SceneView不应该有差异才对的
#if UNITY_EDITOR
                if (camera.cameraType == CameraType.Game)
#endif
                {
                    if (SystemInfo.graphicsUVStartsAtTop)
                    {
                        scaleAndOffsetMatrix.m11 *= -1;    // [-vertical, 0]
                        scaleAndOffsetMatrix.m13 = vertical / 2.0f;  // [-vertical / 2, vertical / 2]
                    }
                }
             
                result = viewToWorldMatrix.transpose * scaleAndOffsetMatrix;
            }
            
            return result;
        }
        
    }
}