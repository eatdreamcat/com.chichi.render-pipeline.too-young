using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;

namespace UnityEngine.Rendering.TooYoung
{
    public partial class TYRenderPipeline : RenderPipeline
    {
        private static readonly string s_PipelineName = "TooYoung RenderPipeline";
        
        private int m_FrameCount;
        private RenderGraph m_RenderGraph = new ("TooYoung RP");

        private GlobalShaderVariables m_GlobalShaderVariables;

        #region Global Settings
        private TYRenderPipelineGlobalSettings m_GlobalSettings;
        #endregion
        
        #region Runtime Resources
        public override RenderPipelineGlobalSettings defaultSettings => m_GlobalSettings;
        
        internal TYRenderPipelineRuntimeMaterials runtimeMaterials { get; private set; }
        internal TYRenderPipelineRuntimeShaders runtimeShaders { get; private set; }
        internal TYRenderPipelineRuntimeAssets runtimeAssets { get; private set; }
        internal TYRenderPipelineRuntimeTextures runtimeTextures { get; private set; }

        #endregion
        
        
        public TYRenderPipeline(TYRenderPipelineAsset asset)
        {
            
            runtimeMaterials = GraphicsSettings.GetRenderPipelineSettings<TYRenderPipelineRuntimeMaterials>();
            runtimeShaders   = GraphicsSettings.GetRenderPipelineSettings<TYRenderPipelineRuntimeShaders>();
            runtimeAssets    = GraphicsSettings.GetRenderPipelineSettings<TYRenderPipelineRuntimeAssets>();
            runtimeTextures  = GraphicsSettings.GetRenderPipelineSettings<TYRenderPipelineRuntimeTextures>();
            
#if UNITY_EDITOR
            // TODO 
            // runtimeShaders.EnsureShadersCompiled();
#endif
            // Initial state of the RTHandle system.
            // We initialize to screen width/height to avoid multiple realloc that can lead to inflated memory usage
            // (as releasing of memory is delayed).
            
            RTHandles.Initialize(Screen.width, Screen.height);
            
            Blitter.Initialize(runtimeShaders.blitPS, runtimeShaders.blitColorAndDepthPS);
            
            InitImplicitRenderingBuffers();
            
            SupportedRenderingFeatures.active.rendersUIOverlay = true;
        }

        void CleanupRenderGraph()
        {
            m_RenderGraph.Cleanup();
            m_RenderGraph = null;
        }
        
        protected override void Dispose(bool disposing)
        {
            ReleaseImplicitRenderingBuffers();
            Blitter.Cleanup();
            CleanupRenderGraph();
            ConstantBuffer.ReleaseAll();
            base.Dispose(disposing);
        }

        #region Private

        bool PrepareAndCullCamera(Camera camera, ScriptableRenderContext renderContext, out CullingResults cullingResults)
        {
            cullingResults = new CullingResults();
            
            if (!camera.TryGetCullingParameters(camera.stereoEnabled, out var cullingParams))
            {
                return false;
            }
            
            return TryCull(camera, renderContext, cullingParams, ref cullingResults);
            
        }

        void InitCameraGlobalConstantBuffer(ref GlobalShaderVariables cb, Camera camera)
        {
            cb._CameraWorldPosition = camera.cameraToWorldMatrix.GetColumn(3);
        }
        
        void SetGlobalConstantBuffer(CommandBuffer cmd, Camera camera)
        {
            InitCameraGlobalConstantBuffer(ref m_GlobalShaderVariables, camera);
            InitImplicitRenderersGlobalConstantBuffer(ref m_GlobalShaderVariables);
            ConstantBuffer.PushGlobal(cmd, m_GlobalShaderVariables, ShaderIDs._GlobalShaderVariables);
        }

        #endregion
        
        #region Public

        

        #endregion

        #region Override

        protected override void Render(ScriptableRenderContext renderContext, Camera[] cameras)
        {
            using (ListPool<Camera>.Get(out var cameraList))
            {
                cameraList.AddRange(cameras);
                InternalRender(renderContext, cameraList);
            }
        }

        #endregion
        
        #region Internal

        internal void InternalRender(ScriptableRenderContext renderContext, List<Camera> cameras)
        {
            
            // TODO: only one game camera for now
            
            BeginContextRendering(renderContext, cameras);

#if UNITY_EDITOR
            int newCount = m_FrameCount;
            foreach (var c in cameras)
            {
                if (c.cameraType != CameraType.Preview)
                {
                    newCount++;
                    break;
                }
            }
#else
            int newCount = Time.frameCount;
#endif
            if (newCount != m_FrameCount)
            {
                m_FrameCount = newCount;
            }
            
            // Rendering Scope
            {
                var cmd = CommandBufferPool.Get(s_PipelineName);
                if (GL.wireframe)
                {
                    // TODO
                    return;
                }

                try
                {
                    foreach (var camera in cameras)
                    {
                        // Here we use the non scaled resolution for the RTHandleSystem
                        // ref size because we assume that at some point we will need full resolution anyway.
                        // This is necessary because we assume that after post processes,
                        // we have the full size render target for debug rendering
                        // The only point of calling this here is to grow the render targets.
                        // The call in BeginRender will setup the current RTHandle viewport size.

                        if (!PrepareAndCullCamera(camera, renderContext, out var cullingResults))
                        {

                            continue;
                        }

                        RTHandles.SetReferenceSize(camera.pixelWidth, camera.pixelHeight);
                        SetGlobalConstantBuffer(cmd, camera);
                        ExecuteWithRenderGraph(renderContext, cmd, camera);

                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Error while building Render Graph.");
                    Debug.LogException(e);
                }
                finally
                {
                    renderContext.ExecuteCommandBuffer(cmd);
                    CommandBufferPool.Release(cmd);
                    renderContext.Submit();
                }
                
                
            }
            
            
            m_RenderGraph.EndFrame();
            
            EndContextRendering(renderContext, cameras);
        }
        
        #endregion 
        
        
        static bool TryCull(
            Camera camera,
            ScriptableRenderContext renderContext,
            ScriptableCullingParameters cullingParams,
            ref CullingResults cullingResults
        )
        {
            if (camera.cameraType == CameraType.Reflection || camera.cameraType == CameraType.Preview)
            {
#if UNITY_2020_2_OR_NEWER
                ScriptableRenderContext.EmitGeometryForCamera(camera);
#endif
            }
#if UNITY_EDITOR
            // emit scene view UI
            else if (camera.cameraType == CameraType.SceneView)
            {
                ScriptableRenderContext.EmitWorldGeometryForSceneView(camera);
            }
#endif
            
            // Set the LOD bias and store current value to be able to restore it.
            // Use a try/finalize pattern to be sure to restore properly the qualitySettings.lodBias
            var initialLODBias = QualitySettings.lodBias;
            var initialMaximumLODLevel = QualitySettings.maximumLODLevel;
            try
            {
                // This needs to be called before culling, otherwise in the case where users generate intermediate renderers, it can provoke crashes.
                BeginCameraRendering(renderContext, camera);
                
                using (new ProfilingScope(ProfilingSampler.Get(TYProfilerId.CullResultsCull)))
                {
                    cullingResults = renderContext.Cull(ref cullingParams);
                }
                
                return true;
            }
            finally
            {
#if UNITY_2021_1_OR_NEWER
                QualitySettings.SetLODSettings(initialLODBias, initialMaximumLODLevel, false);
#else
                QualitySettings.lodBias = initialLODBias;
                QualitySettings.maximumLODLevel = initialMaximumLODLevel;
#endif
            }
        }
    }
}
