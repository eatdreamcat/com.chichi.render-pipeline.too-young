using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering.RenderGraphModule;

namespace UnityEngine.Rendering.TooYoung
{
    internal class RayTraingInOneWeekendPassData
    {
        internal ComputeShader cs;
        internal int kernelIndex;
        internal Vector3Int dispatchSize;
    }
    
    public partial class TYRenderPipeline
    {
        const string k_RayTracingInOneWeekend = "RayTracingInOneWeekend";
        private const string k_TargetRTName = "_Result";
        TextureHandle RenderTracingInOneWeekendPass(Camera camera)
        {
            var desc = new TextureDesc()
            {
                scale = Vector2.one,
                colorFormat = GraphicsFormat.B10G11R11_UFloatPack32,
                dimension = TextureDimension.Tex2D,
                slices = 1,
                sizeMode = TextureSizeMode.Scale,
                msaaSamples = MSAASamples.None,
                name = k_RayTracingInOneWeekend,
                enableRandomWrite = true
            };
            
            var target = m_RenderGraph.CreateTexture(desc);
            
            using (var builder = m_RenderGraph.AddComputePass<RayTraingInOneWeekendPassData>(
                       k_RayTracingInOneWeekend, out var passData, 
                       ProfilingSampler.Get(TYProfilerId.RayTracingInOneWeekend)))
            {
                builder.UseTexture(target, AccessFlags.ReadWrite);
                passData.cs = runtimeShaders.rayTracingInOneWeekendCS;
                passData.kernelIndex = passData.cs.FindKernel(k_RayTracingInOneWeekend);
                passData.dispatchSize = new Vector3Int(
                    camera.scaledPixelWidth,
                    camera.scaledPixelHeight,
                    1
                );
                
                builder.SetRenderFunc((RayTraingInOneWeekendPassData data, ComputeGraphContext ctx) =>
                {
                    ctx.cmd.SetComputeTextureParam(data.cs, data.kernelIndex, k_TargetRTName, target);
                    ctx.cmd.DispatchCompute(data.cs, data.kernelIndex, 
                        data.dispatchSize.x, 
                        data.dispatchSize.y, 
                        data.dispatchSize.z);
                });
            }
            
            return target;
        }
    }

}
