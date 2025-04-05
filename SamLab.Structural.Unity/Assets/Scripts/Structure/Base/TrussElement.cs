using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Structure.Managers;
using UnityEngine;

namespace Assets.Scripts.Structure.Base
{
    public class TrussElement : MonoBehaviour, IStructuralElement
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        [SerializeField] public TrussNode StartNode { get; set; }
        [SerializeField] public TrussNode EndNode { get; set; }
        //[SerializeField] private bool RenderLine = true;

        [Tooltip("Base width of the line in screen pixels")]
        public float screenSpaceWidth = 1f;

        private LineRenderer _lineRenderer;
        private TrussStructure _parentStructure;

        public void Initialize(TrussStructure parentStructure, TrussNode startNode, TrussNode endNode)
        {
            _parentStructure = parentStructure;
            StartNode = startNode;
            EndNode = endNode;
            StartNode.ConnectedElements.Add(this);
            EndNode.ConnectedElements.Add(this);
            _lineRenderer = gameObject.GetComponent<LineRenderer>();
            _lineRenderer.positionCount = 2;
            _lineRenderer.startWidth = screenSpaceWidth;
        }

        // Update is called once per frame
        private void Update()
        {
            if(StartNode == null || EndNode == null)
                return;

            _lineRenderer.SetPosition(0, StartNode.gameObject.transform.position);
            _lineRenderer.SetPosition(1, EndNode.gameObject.transform.position);

            NormalizeLineWidth();
        }

        public void SetStartNode(TrussNode newStartNode)
        {
            if (StartNode != null)
                StartNode.RemoveConnectedElement(this);

            StartNode = newStartNode;

            if (StartNode != null)
                StartNode.AddConnectedElement(this);
        }

        public void SetEndNode(TrussNode newEndNode)
        {
            if (EndNode != null)
                EndNode.RemoveConnectedElement(this);

            EndNode = newEndNode;

            if (EndNode != null)
                EndNode.AddConnectedElement(this);
        }

        private void NormalizeLineWidth()
        {
            //var midPoint = (StartNode.transform.position + EndNode.transform.position) / 2f;
            //var distanceToCamera = Vector3.Distance(midPoint, Camera.main.transform.position);

            //var worldSpaceWidth = screenSpaceWidth * distanceToCamera / 1000f;

            //_lineRenderer.startWidth = worldSpaceWidth;
            //_lineRenderer.endWidth = worldSpaceWidth;
        }

    }
}