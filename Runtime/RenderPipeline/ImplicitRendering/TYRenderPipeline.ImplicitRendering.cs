using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering.RenderGraphModule;

namespace UnityEngine.Rendering.TooYoung
{
    internal class ImplicitRenderingPassData
    {
        internal ComputeShader cs;
        internal int kernelIndex;
        internal Vector3Int dispatchSize;
    }
    
    public partial class TYRenderPipeline
    {
        const string k_ImplicitRendering = "ImplicitRendering";
        private const string k_TargetRTName = "_Result";
        TextureHandle ImplicitRenderingPass(Camera camera)
        {
            var desc = new TextureDesc()
            {
                scale = Vector2.one,
                colorFormat = GraphicsFormat.B10G11R11_UFloatPack32,
                dimension = TextureDimension.Tex2D,
                slices = 1,
                sizeMode = TextureSizeMode.Scale,
                msaaSamples = MSAASamples.None,
                name = k_ImplicitRendering,
                enableRandomWrite = true
            };
            
            var target = m_RenderGraph.CreateTexture(desc);
            
            using (var builder = m_RenderGraph.AddComputePass<ImplicitRenderingPassData>(
                       k_ImplicitRendering, out var passData, 
                       ProfilingSampler.Get(TYProfilerId.ImplicitRendering)))
            {
                builder.UseTexture(target, AccessFlags.ReadWrite);
                passData.cs = runtimeShaders.implicitRendeirngCS;
                passData.kernelIndex = passData.cs.FindKernel(k_ImplicitRendering);
                passData.dispatchSize = new Vector3Int(
                    camera.scaledPixelWidth,
                    camera.scaledPixelHeight,
                    1
                );
                
                builder.SetRenderFunc((ImplicitRenderingPassData data, ComputeGraphContext ctx) =>
                {
                    var pixelToWorldMatrix = TYUtils.ComputePixelCoordToWorldSpaceViewDirectionMatrix(
                        camera,
                        new Vector4(
                            camera.scaledPixelWidth, 
                            camera.scaledPixelHeight,
                            1.0f / camera.scaledPixelWidth, 
                            1.0f / camera.scaledPixelHeight
                            )
                        );
                    ctx.cmd.SetComputeMatrixParam(data.cs, Shader.PropertyToID("_PixelCoordToWorldMatrix"), pixelToWorldMatrix);
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
