
namespace UnityEngine.Rendering.TooYoung
{
    public static partial class TYUtils
    {
        public static void DrawFullScreen(CommandBuffer commandBuffer, Rect viewport, Material material, RenderTargetIdentifier destination, CubemapFace cubemapFace, MaterialPropertyBlock properties = null, int shaderPassId = 0, int depthSlice = -1)
        {
            CoreUtils.SetRenderTarget(commandBuffer, destination, ClearFlag.None, 0, cubemapFace, depthSlice);
            commandBuffer.SetViewport(viewport);
            commandBuffer.DrawProcedural(Matrix4x4.identity, material, shaderPassId, MeshTopology.Triangles, 3, 1, properties);
        }

        public static RenderTargetIdentifier GetCameraTargetId(Camera camera)
        {
            // Select render target
            RenderTargetIdentifier targetId =
                camera.targetTexture ?? new RenderTargetIdentifier(BuiltinRenderTextureType.CameraTarget);
            return targetId;
        }
    }
    
}

