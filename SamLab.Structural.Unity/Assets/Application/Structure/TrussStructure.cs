using System.Collections.Generic;
using Assets.Application.Workspace;
using UnityEngine;

namespace Assets.Application.Structure
{
    public class TrussStructure : MonoBehaviour
    {
        [SerializeField] public List<TrussNode> Nodes { get; set; }
        [SerializeField] public List<TrussElement> Members;
        [SerializeField] public List<TrussConstraint> Supports;
        [SerializeField] public float NodeSnapTolerance = 0.1f;

        private TrussFactory _trussFactory;
        private TrussManager _trussManager;

        public SnapHandler SnapHandler { get; private set; }

        private List<TrussNode> _selectedNodes;
        private List<TrussElement> _selectedMembers;

        public void Initialize(TrussManager manager)
        {
            Nodes ??= new List<TrussNode>();
            Members ??= new List<TrussElement>();
            Supports ??= new List<TrussConstraint>();

            _trussManager = manager;
            _trussFactory = new TrussFactory();
            SnapHandler = new SnapHandler(this);
        }

        public void CreateNode(Vector3 position)
        {
            var node = _trussFactory.CreateNode(position, this);
            AddNode(node);
        }

        public void CreateMember(TrussNode startNode, TrussNode endNode)
        {
            var element = _trussFactory.CreateMember(startNode, endNode, this);
            AddMember(element);
        }

        public void CreateMember(Vector3 start, Vector3 end)
        {
            var startNode = _trussFactory.CreateNode(start, this);
            var endNode = _trussFactory.CreateNode(end, this);
            AddNode(startNode);
            AddNode(endNode);
            CreateMember(startNode, endNode);
        }

        public void DeleteNode(TrussNode node)
        {
            RemoveNode(node);
            Destroy(node.gameObject);
        }

        public void DeleteMember(TrussElement element)
        {
            RemoveMember(element);
            Destroy(element.gameObject);
        }

        private void AddNode(TrussNode node)
        {
            if (Nodes.Contains(node))
                return;

            Nodes.Add(node);
        }

        public void RemoveNode(TrussNode node)
        {
            if (!Nodes.Contains(node))
                return;
            Nodes.Remove(node);
        }

        private void AddMember(TrussElement element)
        {
            if (Members.Contains(element))
                return;
            Members.Add(element);
        }

        private void RemoveMember(TrussElement member)
        {
            if (!Members.Contains(member))
                return;
            Members.Remove(member);
        }

        private void AddSupport(TrussConstraint support)
        {
            if (Supports.Contains(support))
                return;
            Supports.Add(support);
        }

        private void RemoveSupport(TrussConstraint support)
        {
            if (!Supports.Contains(support))
                return;
            Supports.Remove(support);
        }

        public void OnNodeReleased(TrussNode releasedNode)
        {
            //check if any nodes are nearby and merge them, or replace one of the nodes with the dragged one, making sure that all the forces and constraints are kept

        }



    }
}