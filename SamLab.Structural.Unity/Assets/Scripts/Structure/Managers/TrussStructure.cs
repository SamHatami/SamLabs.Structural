using System.Collections.Generic;
using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Structure.Base;
using Assets.Scripts.Structure.Base.Constraints;
using Assets.Scripts.Structure.Factories;
using Assets.Scripts.Workspace.Geometry.ReferenceGeometry;
using Assets.Scripts.Workspace.Managers;
using UnityEngine;

namespace Assets.Scripts.Structure.Managers
{
    public class TrussStructure : MonoBehaviour
    {
        [SerializeField] public List<ISharedNode> SharedNodes;
        [SerializeField] public List<TrussNode> Nodes;
        [SerializeField] public List<TrussElement> Members;
        [SerializeField] public List<TrussConstraint> Supports;
        [SerializeField] public float NodeSnapTolerance = 0.1f;
        [SerializeField] public WorkPlane ReferenceWorkPlane;
        private TrussFactory _trussFactory;
        private TrussManager _trussManager;

        public WorkspaceSnapHandler WorkspaceSnapHandler { get; private set; }

        private List<TrussNode> _selectedNodes;
        private List<TrussElement> _selectedMembers;

        public void Initialize(TrussManager manager, TrussFactory trussFactory)
        {
            Nodes ??= new List<TrussNode>();
            Members ??= new List<TrussElement>();
            Supports ??= new List<TrussConstraint>();

            _trussManager = manager;
            _trussFactory = trussFactory;
            WorkspaceSnapHandler = FindFirstObjectByType<WorkspaceSnapHandler>();
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
    }
}