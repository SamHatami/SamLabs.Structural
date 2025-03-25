using Assets.Scripts.Workspace.Geometry.Interfaces;
using Codice.CM.Common.Tree;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Assets.Scripts.Workspace.Geometry.ReferenceGeometry
{
    public class WorkAxis : MonoBehaviour, IReferenceGeometry
    {

        private readonly float _pixelWidth = 2f;
        private LineRenderer _lineRenderer;


        public void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            if (_lineRenderer == null)
                _lineRenderer = gameObject.AddComponent<LineRenderer>();

        }
        public void Initialize()
        {

            // Configure the LineRenderer
            _lineRenderer.positionCount = 2;
        }

        public string Name { get; set; }
        public bool IsVisible { get; set; } = true;
        public bool IsActive { get; set; }


        void Update()
        {
            if (IsVisible)
            {
                NormalizeLineWidth();
                _lineRenderer.enabled = true;
            }
            else
            {
                _lineRenderer.enabled = false;
            }
        }

        private void NormalizeLineWidth()
        {
            Vector3 midPoint = transform.position;

            // Get distance from camera to the midpoint
            float distanceToCamera = Vector3.Distance(midPoint, UnityEngine.Camera.main.transform.position);

            // Calculate width based on screen space size and distance
            float worldSpaceWidth = _pixelWidth * distanceToCamera / 1000f;

            // Apply the calculated width to line renderer
            _lineRenderer.startWidth = worldSpaceWidth;
            _lineRenderer.endWidth = worldSpaceWidth;
        }
    }
}