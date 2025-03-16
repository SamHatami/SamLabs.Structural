using UnityEngine;

namespace Assets.Application.Structure
{
    public class TrussMember : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        [SerializeField] private GameObject _startNode;
        [SerializeField] private GameObject _endNode;
        [SerializeField] private bool RenderLine = true;

        [Tooltip("Base width of the line in screen pixels")]
        public float screenSpaceWidth = 0.1f;

        private LineRenderer _lineRenderer;


        public void Initialize(TrussManager manager, TrussNode startNode, TrussNode endNode)
        {
            _startNode = startNode.gameObject;
            _endNode = endNode.gameObject;
            _lineRenderer = gameObject.GetComponent<LineRenderer>();
            _lineRenderer.positionCount = 2;
            _lineRenderer.startWidth = 0.1f;
        }

        // Update is called once per frame
        private void Update()
        {
            _lineRenderer.SetPosition(0, _startNode.gameObject.transform.position);
            _lineRenderer.SetPosition(1, _endNode.gameObject.transform.position);

            NormalizeLineWidth();
        }

        private void NormalizeLineWidth()
        {
            void AdjustLineWidth()
            {
                // Calculate the average distance from camera to line
                Vector3 midPoint = (_startNode.transform.position + _endNode.transform.position) / 2f;
                float distanceToCamera = Vector3.Distance(midPoint, Camera.main.transform.position);

                // Convert screen space width to world space width at this distance
                float worldSpaceWidth = screenSpaceWidth * distanceToCamera / 1000f;

                // Apply to line renderer
                _lineRenderer.startWidth = worldSpaceWidth;
                _lineRenderer.endWidth = worldSpaceWidth;
            }
        }
    }
}