using UnityEngine;

namespace Workspace.Interaction
{
    public class HoverColorChange : MonoBehaviour
    {
        [Header("Border Settings")] public Color normalBorderColor = Color.white;
        public Color hoverBorderColor = Color.yellow;
        [Range(0, 1)] public float normalBorderAlpha = 1.0f;
        [Range(0, 1)] public float hoverBorderAlpha = 1.0f;

        [Header("Center Settings")] public Color normalCenterColor = new(0.5f, 0.5f, 0.5f);
        public Color hoverCenterColor = new(0.6f, 0.6f, 0.6f);
        [Range(0, 1)] public float normalCenterAlpha = 0.2f;
        [Range(0, 1)] public float hoverCenterAlpha = 0.4f;

        [Header("Property Names")] public string borderColorPropertyName = "_BorderColor";
        public string centerColorPropertyName = "_MainColor";

        private Material material;
        private UnityEngine.Camera mainCamera;
        private Renderer rend;
        private Collider planeCollider;

        private void Start()
        {
            // Get the material and make a unique instance
            rend = GetComponent<Renderer>();
            material = rend.material; // This creates a material instance

            // Cache main camera
            mainCamera = UnityEngine.Camera.main;

            // Ensure we have a collider for raycasting
            planeCollider = GetComponent<Collider>();
            if (planeCollider == null)
                // If no collider, add one
                planeCollider = gameObject.AddComponent<BoxCollider>();

            // Set initial colors
            SetNormalColors();
        }

        private void Update()
        {
            // Cast ray from mouse position
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if mouse is over this plane
            if (Physics.Raycast(ray, out hit) && hit.collider == planeCollider)
                // Mouse is over the plane, change to hover colors
                SetHoverColors();
            else
                // Mouse is not over the plane, revert to normal colors
                SetNormalColors();
        }

        private void SetNormalColors()
        {
            // Set border color with alpha
            var borderColor = normalBorderColor;
            borderColor.a = normalBorderAlpha;
            material.SetColor(borderColorPropertyName, borderColor);

            // Set center color with alpha
            var centerColor = normalCenterColor;
            centerColor.a = normalCenterAlpha;
            material.SetColor(centerColorPropertyName, centerColor);
        }

        private void SetHoverColors()
        {
            // Set border color with alpha
            var borderColor = hoverBorderColor;
            borderColor.a = hoverBorderAlpha;
            material.SetColor(borderColorPropertyName, borderColor);

            // Set center color with alpha
            var centerColor = hoverCenterColor;
            centerColor.a = hoverCenterAlpha;
            material.SetColor(centerColorPropertyName, centerColor);
        }

        // Clean up to prevent memory leaks
        private void OnDestroy()
        {
            // Destroy the material instance
            if (material != null) Destroy(material);
        }
    }
}