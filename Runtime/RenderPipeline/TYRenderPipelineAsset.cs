using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEngine.Rendering.TooYoung
{
    public class TYRenderPipelineAsset : RenderPipelineAsset<TYRenderPipeline>,
        IVirtualTexturingEnabledRenderPipeline, IProbeVolumeEnabledRenderPipeline, IGPUResidentRenderPipeline
    {
        protected override RenderPipeline CreatePipeline()
        {
            var renderPipeline = new TYRenderPipeline(this);

            IGPUResidentRenderPipeline.ReinitializeGPUResidentDrawer();

            return renderPipeline;
        }

        public bool virtualTexturingEnabled { get; }
        public bool supportProbeVolume { get; }
        public ProbeVolumeSHBands maxSHBands { get; }
        public ProbeVolumeSceneData probeVolumeSceneData { get; }
        public GPUResidentDrawerSettings gpuResidentDrawerSettings { get; }
        public GPUResidentDrawerMode gpuResidentDrawerMode { get; set; }
    }
}
