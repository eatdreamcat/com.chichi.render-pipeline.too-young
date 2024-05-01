using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;

namespace UnityEngine.Rendering.TooYoung
{
    public partial class TYRenderPipeline
    {
        const string k_FinalBlitPass = "FinalBlitPass";
        internal class FinalBlitPassData
        {
            internal TextureHandle source;
            internal TextureHandle destination;
        }
        
        #region Private

        void SetupCameraProperties(Camera camera, ScriptableRenderContext renderContext,CommandBuffer commandBuffer)
        {
            // The next 2 functions are required to flush the command buffer before calling functions directly on the render context.
            // This way, the commands will execute in the order specified by the C# code.
            renderContext.ExecuteCommandBuffer(commandBuffer);
            commandBuffer.Clear();

            renderContext.SetupCameraProperties(camera, false);
        }
        
        private void ExecuteWithRenderGraph(ScriptableRenderContext renderContext,CommandBuffer commandBuffer, Camera camera)
        {
            var parameters = new RenderGraphParameters
            {
                executionName = camera.name,
                currentFrameIndex = m_FrameCount,
                rendererListCulling = true,
                scriptableRenderContext = renderContext,
                commandBuffer = commandBuffer
            };
            commandBuffer.DisableScissorRect();
            m_RenderGraph.BeginRecording(parameters);
            SetupCameraProperties(camera, renderContext, commandBuffer);
            RecordRenderGraph(renderContext, commandBuffer, camera);
            m_RenderGraph.EndRecordingAndExecute();
        }

        private void RecordRenderGraph(ScriptableRenderContext renderContext, CommandBuffer commandBuffer,
            Camera camera)
        {
            using (new ProfilingScope(commandBuffer, ProfilingSampler.Get(TYProfilerId.RecordRenderGraph)))
            {
                var backBuffer = RenderImplicitObjectToCameraTarget(camera);
                // Render gizmos that should be affected by post processes
                RenderGizmos(m_RenderGraph, camera, GizmoSubset.PreImageEffects);
                // TODO : post processing
                RenderWireOverlay(m_RenderGraph, camera, backBuffer);
                RenderGizmos(m_RenderGraph, camera, GizmoSubset.PostImageEffects);
            }
        }

        
        TextureHandle BlitToFinalCameraTexture2D(Camera camera, TextureHandle source,
            TextureHandle destination)
        {
            using (var builder = m_RenderGraph.AddRenderPass<FinalBlitPassData>(
                       k_FinalBlitPass, out var passData, ProfilingSampler.Get(TYProfilerId.BlitToFinalCameraTexture)))
            {
                passData.source = builder.ReadTexture(source);
                passData.destination = builder.WriteTexture(destination);
                builder.SetRenderFunc((FinalBlitPassData data, RenderGraphContext ctx) =>
                {
                    Blitter.BlitCameraTexture2D(ctx.cmd, data.source, data.destination);
                });
            }

            return destination;
        }

        void RenderScreenSpaceOverlayUI(RenderGraph renderGraph, Camera camera, TextureHandle colorBuffer)
        {
            // TODO
        }
        
        
        #endregion

        #region Gizmos

#if UNITY_EDITOR
        class RenderWireOverlayPassData
        {
            public Camera camera;
        }
#endif
        
        void RenderWireOverlay(RenderGraph renderGraph, Camera camera, TextureHandle colorBuffer)
        {
#if UNITY_EDITOR
            if (camera.cameraType == CameraType.SceneView)
            {
                using (var builder = renderGraph.AddRenderPass<RenderWireOverlayPassData>("Wire Overlay",
                           out var passData))
                {
                    builder.WriteTexture(colorBuffer);
                    passData.camera = camera;

                    builder.SetRenderFunc(
                        (RenderWireOverlayPassData data, RenderGraphContext ctx) =>
                        {
                            ctx.renderContext.ExecuteCommandBuffer(ctx.cmd);
                            ctx.cmd.Clear();
                            ctx.renderContext.DrawWireOverlay(camera);
                        });
                }
            }
#endif
        }
        
#if UNITY_EDITOR
        class RenderGizmosPassData
        {
            public GizmoSubset  gizmoSubset;
            public Camera       camera;
            public Texture      exposureTexture;
        }
#endif
        
        void RenderGizmos(RenderGraph renderGraph, Camera camera, GizmoSubset gizmoSubset)
        {
#if UNITY_EDITOR
            if (UnityEditor.Handles.ShouldRenderGizmos() &&
                (camera.cameraType == CameraType.Game || camera.cameraType == CameraType.SceneView))
            {
                bool renderPrePostprocessGizmos = (gizmoSubset == GizmoSubset.PreImageEffects);
                using (var builder = renderGraph.AddRenderPass<RenderGizmosPassData>
                           (renderPrePostprocessGizmos ? "PrePostprocessGizmos" : "Gizmos", out var passData))
                {
                    passData.gizmoSubset = gizmoSubset;
                    passData.camera = camera;

                    builder.SetRenderFunc(
                        (RenderGizmosPassData data, RenderGraphContext ctx) =>
                        {
                            // TODO: Gizmos exposure
                            ctx.renderContext.ExecuteCommandBuffer(ctx.cmd);
                            ctx.cmd.Clear();
                            ctx.renderContext.DrawGizmos(data.camera, data.gizmoSubset);
                        });
                }
            }
#endif
        }
        
        #endregion
    }
}