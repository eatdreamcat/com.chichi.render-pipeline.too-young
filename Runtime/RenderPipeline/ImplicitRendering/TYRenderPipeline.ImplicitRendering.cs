using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering.RenderGraphModule;

namespace UnityEngine.Rendering.TooYoung
{
    internal class ImplicitRenderingPassData
    {
        internal ComputeShader cs;
        internal int renderKernelIndex;
        internal int clearKernelIndex;
        internal Vector3Int dispatchSize;
    }
    
    public partial class TYRenderPipeline
    {
        const string k_ImplicitRendering = "ImplicitRendering";
        private const string k_ImlicitClear = "Clear";
        private const string k_TargetRTName = "_Result";

        private ComputeBuffer m_ImplicitRenderersBuffer;

        void InitImplicitRenderingBuffers()
        {
            var stride =  System.Runtime.InteropServices.Marshal.SizeOf(typeof(ImplicitRendererInfo));
            m_ImplicitRenderersBuffer = new ComputeBuffer(
                ConstDefine.k_MAX_IMPLICIT_RENDERER_COUNT, stride
                , ComputeBufferType.Structured);
        }

        void ReleaseImplicitRenderingBuffers()
        {
            if (m_ImplicitRenderersBuffer != null)
            {
                CoreUtils.SafeRelease(m_ImplicitRenderersBuffer);
                m_ImplicitRenderersBuffer = null;
            }
        }

        unsafe void InitImplicitRenderersGlobalConstantBuffer(ref GlobalShaderVariables constantBuffer)
        {
            ref var cb = ref constantBuffer;
            for (int i = 0; i < Sphere.s_ImplicitSpheres.Count; ++i)
            {
                var sphere = Sphere.s_ImplicitSpheres[i];
                var sphereInfo = new Vector4(
                    sphere.Center.x,
                    sphere.Center.y,
                    sphere.Center.z,
                    sphere.Radius
                    );
                for (int offset = 0; offset < ConstDefine.k_IMPLICIT_SPHERE_STRIDE; ++offset)
                {
                    var value = sphereInfo[offset];
                    cb._ImplicitSphereList[i * ConstDefine.k_IMPLICIT_SPHERE_STRIDE + offset] = value;
                }
            }
            
        }
        
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
                       k_ImplicitRendering, out var passData, ProfilingSampler.Get(TYProfilerId.ImplicitRendering)))
            {
                builder.UseTexture(target, AccessFlags.ReadWrite);
                passData.cs = runtimeShaders.implicitRendeirngCS;
                passData.renderKernelIndex = passData.cs.FindKernel(k_ImplicitRendering);
                passData.clearKernelIndex = passData.cs.FindKernel(k_ImlicitClear);
                // TODO: size should match the actual RT size
                passData.dispatchSize = new Vector3Int(
                    camera.scaledPixelWidth,
                    camera.scaledPixelHeight,
                    1
                );
                
                // fill implicit renderer list buffer
                using (ListPool<ImplicitRendererInfo>.Get(out var rendererInfos))
                {
                    for(int index = 0; index < ImplicitRenderer.s_ImplicitRendererList.Count; ++index)
                    {
                        var renderer = ImplicitRenderer.s_ImplicitRendererList[index];
                        // TODO: optimize this bullshit code
                        if (renderer is Sphere)
                        {
                            rendererInfos.Add(new ImplicitRendererInfo()
                            {
                                primitive = (int)renderer.GetPrimitiveType(),
                                instanceId = Sphere.s_ImplicitSpheres.IndexOf((renderer as Sphere))
                            });
                        }
                    }
                    m_ImplicitRenderersBuffer.SetData(rendererInfos);
                }
                
                builder.SetRenderFunc((ImplicitRenderingPassData data, ComputeGraphContext ctx) =>
                {
                    using (new ProfilingScope(ProfilingSampler.Get(TYProfilerId.ImplicitClear)))
                    {
                        // Clear
                        ctx.cmd.SetComputeVectorParam(data.cs, ShaderIDs._ClearColor, new Vector4(0, 0,0, 1));
                        ctx.cmd.SetComputeTextureParam(data.cs, data.clearKernelIndex, k_TargetRTName, target);
                        ctx.cmd.DispatchCompute(data.cs, data.clearKernelIndex, 
                            data.dispatchSize.x, 
                            data.dispatchSize.y, 
                            data.dispatchSize.z);
                    }

                    using (new ProfilingScope(ProfilingSampler.Get(TYProfilerId.ImplicitShading)))
                    {
                        // Render Implicit Primitives
                        var pixelToWorldMatrix = TYUtils.ComputePixelCoordToWorldSpaceViewDirectionMatrix(
                            camera,
                            new Vector4(
                                camera.scaledPixelWidth, 
                                camera.scaledPixelHeight,
                                1.0f / camera.scaledPixelWidth, 
                                1.0f / camera.scaledPixelHeight
                            )
                        );
                        ctx.cmd.SetComputeMatrixParam(data.cs, ShaderIDs._PixelCoordToViewDirWS, pixelToWorldMatrix);
                        ctx.cmd.SetComputeTextureParam(data.cs, data.renderKernelIndex, k_TargetRTName, target);
                        ctx.cmd.SetComputeBufferParam(data.cs, data.renderKernelIndex, 
                            ShaderIDs._ImplicitRendererList, m_ImplicitRenderersBuffer);
                        ctx.cmd.SetComputeFloatParam(data.cs, ShaderIDs._ImplicitRenderersCount,
                            ImplicitRenderer.s_ImplicitRendererList.Count);
                        ctx.cmd.DispatchCompute(data.cs, data.renderKernelIndex, 
                            data.dispatchSize.x, 
                            data.dispatchSize.y, 
                            data.dispatchSize.z);
                    }
                });
            }
            
            return target;
        }

        TextureHandle RenderImplicitObjectToCameraTarget(Camera camera)
        {
            var backBuffer = m_RenderGraph.ImportBackbuffer(TYUtils.GetCameraTargetId(camera));
                
            // RaytracingSeries
            var rayTracingResult = ImplicitRenderingPass(camera);
            return BlitToFinalCameraTexture2D(camera, rayTracingResult, backBuffer);
        }
    }

}
