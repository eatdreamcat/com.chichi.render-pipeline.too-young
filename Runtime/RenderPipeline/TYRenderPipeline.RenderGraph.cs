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

            m_RenderGraph.BeginRecording(parameters);
            RecordRenderGraph(renderContext, commandBuffer, camera);
            m_RenderGraph.EndRecordingAndExecute();
        }

        private void RecordRenderGraph(ScriptableRenderContext renderContext, CommandBuffer commandBuffer,
            Camera camera)
        {
            using (new ProfilingScope(commandBuffer, ProfilingSampler.Get(TYProfilerId.RecordRenderGraph)))
            {
                var backBuffer = m_RenderGraph.ImportBackbuffer(TYUtils.GetCameraTargetId(camera));
                
                // RaytracingSeries
                var rayTracingResult = ImplicitRenderingPass(camera);
                BlitToFinalCameraTexture2D(camera, rayTracingResult, backBuffer);
            }
        }

        
        void BlitToFinalCameraTexture2D(Camera camera, TextureHandle source,
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
        }

        #endregion
    }
}