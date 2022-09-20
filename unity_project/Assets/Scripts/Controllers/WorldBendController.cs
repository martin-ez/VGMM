using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Controllers
{
    [ExecuteAlways]
    public class WorldBendController : MonoBehaviour
    {
        private void Awake()
        {
            if (Application.isPlaying) Shader.EnableKeyword("_ENABLE_WORLD_BEND");
            else Shader.DisableKeyword("_ENABLE_WORLD_BEND");
        }

        private void OnEnable()
        {
            RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
            RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
        }

        private void OnDisable()
        {
            RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
            RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
        }

        private void OnBeginCameraRendering(ScriptableRenderContext context, Camera cam)
        {
            cam.cullingMatrix = Matrix4x4.Ortho(-99, 99, -99, 99, 0.001f, 1000) * cam.worldToCameraMatrix;
        }

        private void OnEndCameraRendering(ScriptableRenderContext context, Camera cam)
        {
            cam.ResetCullingMatrix();
        }
    }
}