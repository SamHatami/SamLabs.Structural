using UnityEngine;

namespace Assets.Scripts.Workspace.Camera
{
    [ExecuteInEditMode] // Makes the script run in the editor too
    [RequireComponent(typeof(UnityEngine.Camera))] // Ensures the script is attached to a Camera
    public class ScreenSpaceOutline : MonoBehaviour
    {
        public Material outlineMaterial; // Assign the material with the shader in the Inspector

        private UnityEngine.Camera _camera;

        private void OnEnable()
        {
            _camera = GetComponent<UnityEngine.Camera>();
            // IMPORTANT: Enable depth and normals texture
            _camera.depthTextureMode = DepthTextureMode.DepthNormals;
        }


        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (outlineMaterial == null)
            {
                Graphics.Blit(source, destination); // Just copy if no material
                return;
            }

            // 1. Create a temporary RenderTexture
            RenderTexture tempRT = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);

            // 2. Pass 1: Edge Detection (render to tempRT)
            Graphics.Blit(source, tempRT, outlineMaterial, 0);

            // 3. Pass 2: Blur and Apply (render to final destination)
            // Pass in the source texture, since the second pass needs it.
            outlineMaterial.SetTexture("_SourceTex", source);
            Graphics.Blit(tempRT, destination, outlineMaterial, 1);

            // 4. Release the temporary RenderTexture
            RenderTexture.ReleaseTemporary(tempRT);
        }
    }
}