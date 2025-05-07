using Core.Interfaces;
using Structure.Managers;
using UnityEngine;

namespace Structure.Base
{
    public class TrussMember : MonoBehaviour, IStructuralElement
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public string Name
        {
            get => name;
            set => name = value;
        }
        [SerializeField] public TrussNode StartNode { get; set; }
        [SerializeField] public TrussNode EndNode { get; set; }
        //[SerializeField] private bool RenderLine = true;

        [Tooltip("Base width of the line in screen pixels")]
        private TrussStructure _parentStructure;

        private Vector3 oldEndNodePosition;
        private Vector3 oldStartNodePosition;
        private float length;

        public void Initialize(TrussStructure parentStructure, TrussNode startNode, TrussNode endNode)
        {
            _parentStructure = parentStructure;
            StartNode = startNode;
            EndNode = endNode;
            StartNode.ConnectedElements.Add(this);
            EndNode.ConnectedElements.Add(this);
            length = Vector3.Distance(StartNode.transform.position, EndNode.transform.position);
        }


        private void Start()
        {
            UpdateScaleAndRotation();
        }

        // Update is called once per frame
        private void Update()
        {
            if (StartNode == null || EndNode == null)
                return;

            if (oldEndNodePosition == EndNode.transform.position &&
                oldStartNodePosition == StartNode.transform.position)
                return;

            oldEndNodePosition = EndNode.transform.position;
            oldStartNodePosition = StartNode.transform.position;

            UpdateScaleAndRotation();
        }

        private void UpdateScaleAndRotation()
        {
            length = Vector3.Distance(StartNode.transform.position, EndNode.transform.position);
            transform.position = StartNode.transform.position;
            transform.LookAt(EndNode.transform);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, length);
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


    }
}