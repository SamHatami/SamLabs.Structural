using UnityEngine;

namespace Assets.Scripts.Workspace
{
    public class InfiniteGrid : MonoBehaviour
    {
        public UnityEngine.Camera mainCamera;
        public Material gridMaterial;
        public float baseGridSize = 100f;
        public float gridCellSize = 0.025f;
        public float minPixelsBetweenCells = 2.0f;

        [Tooltip("Multiplier for grid size based on orthographic size")]
        public float orthoSizeMultiplier = 3f;

        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;
        private float previousOrthographicSize;

        void Start()
        {
            if (mainCamera == null)
                mainCamera = UnityEngine.Camera.main;

            previousOrthographicSize = mainCamera.orthographicSize;

            // Create simple quad mesh for the grid
            meshFilter = gameObject.AddComponent<MeshFilter>();
            meshRenderer = gameObject.AddComponent<MeshRenderer>();

            Mesh mesh = new Mesh();
            mesh.vertices = new Vector3[] {
                new Vector3(-1, 0, -1),
                new Vector3(1, 0, -1),
                new Vector3(1, 0, 1),
                new Vector3(-1, 0, 1)
            };
            mesh.triangles = new int[] { 0, 2, 1, 0, 3, 2 };
            mesh.uv = new Vector2[] {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(1, 1),
                new Vector2(0, 1)
            };
            mesh.RecalculateNormals();
            meshFilter.mesh = mesh;

            // Assign material
            if (gridMaterial != null)
                meshRenderer.material = gridMaterial;
            else
                Debug.LogError("Grid material not assigned!");

            // Initialize grid properties
            UpdateGridProperties();

            // Initial scale update based on camera type
            UpdateGridScale();
        }

        void Update()
        {
            // Update position to follow camera
            Vector3 camPos = mainCamera.transform.position;
            transform.position = new Vector3(camPos.x, 0, camPos.z);
            transform.rotation = Quaternion.Euler(90, 0, 0);

            // For orthographic cameras, adjust grid scale based on orthographic size
            if (mainCamera.orthographic && Mathf.Abs(mainCamera.orthographicSize - previousOrthographicSize) > 0.01f)
            {
                UpdateGridScale();
                previousOrthographicSize = mainCamera.orthographicSize;
            }
        }

        void UpdateGridScale()
        {
            float targetGridSize = baseGridSize;

            if (mainCamera.orthographic)
            {
                // For orthographic, scale the grid plane size based on orthographic size
                float orthoSize = Mathf.Abs(mainCamera.orthographicSize);
                targetGridSize = baseGridSize + (orthoSize * orthoSizeMultiplier);
            }

            // Update the physical scale of the grid plane
            transform.localScale = new Vector3(targetGridSize, targetGridSize, 1);

            // This is important: update the _GridSize parameter in shader to match physical size
            // so fade effects work correctly
            if (gridMaterial != null)
            {
                gridMaterial.SetFloat("_GridSize", targetGridSize);
            }
        }

        void UpdateGridProperties()
        {
            if (gridMaterial != null)
            {
                // Set initial material properties - cell size doesn't change
                gridMaterial.SetFloat("_GridCellSize", gridCellSize);
                gridMaterial.SetFloat("_GridMinPixelsBetweenCells", minPixelsBetweenCells);
            }
        }
    }
}