using UnityEngine;
namespace Assets.Scripts.Workspace.UI
{
    //Source https://discussions.unity.com/t/in-game-infinite-grids/604479/5
    public class DynamicGrid : MonoBehaviour
    {
        public Material material;
        [Range(0.0001f, 0.01f)]
        public float referenceThickness = 0.005f;
        [Range(1f, 50f)]
        public float referenceOrthoSize = 5f;
        public float gridHeight = 0f; // Height of the grid (Y position)
        public bool allowGridMovement = true; // Toggle to enable/disable grid following

        private UnityEngine.Camera mainCamera;

        void Awake()
        {
            mainCamera = UnityEngine.Camera.main;
            AdjustLineThickness();
        }

        void Update()
        {
            AdjustLineThickness();

            if (allowGridMovement)
            {
                PositionGridWithCamera();
            }

            // Always make the grid face the camera regardless of position
            transform.LookAt(mainCamera.transform);
        }

        void AdjustLineThickness()
        {
            float currentOrtho = mainCamera.orthographicSize;
            float ratio = currentOrtho / referenceOrthoSize;
            float adjustedThickness = referenceThickness * ratio;
            adjustedThickness = Mathf.Clamp(adjustedThickness, 0.0001f, 0.01f);
            material.SetFloat("_Thickness", adjustedThickness);
        }

        void PositionGridWithCamera()
        {
            // Only adjust grid position when camera is at right angles
            // Check if camera is looking straight down (or nearly so)
            float dotProduct = Vector3.Dot(mainCamera.transform.forward, Vector3.down);

            if (dotProduct > 0.9f) // 0.9 corresponds to roughly 25 degrees from vertical
            {
                // Camera is looking down at a steep angle, position grid below camera
                Vector3 gridPosition = new Vector3(
                    mainCamera.transform.position.x,
                    gridHeight,
                    mainCamera.transform.position.z
                );

                // Cast a ray down from the camera to find intersection with the floor plane
                Ray ray = new Ray(mainCamera.transform.position, Vector3.down);
                Plane floorPlane = new Plane(Vector3.up, new Vector3(0, gridHeight, 0));

                float distance;
                if (floorPlane.Raycast(ray, out distance))
                {
                    // Position grid at intersection point
                    gridPosition = ray.GetPoint(distance);
                }

                transform.position = gridPosition;
            }
        }
    }
}