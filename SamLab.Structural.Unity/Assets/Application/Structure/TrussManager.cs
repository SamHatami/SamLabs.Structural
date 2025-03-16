using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Application.Structure
{
    public class TrussManager : MonoBehaviour
    {
        [SerializeField] private List<TrussNode> _nodes;
        [SerializeField] private List<TrussMember> _members;

        private List<TrussNode> _selectedNodes;
        private List<TrussMember> _selectedMembers;
        private TrussFactory _trussFactory;

        // Start is called once before the first execution of Update after the MonoBehaviour is created

        
        private void Start()
        {
            _nodes ??= new List<TrussNode>();
            _members ??= new List<TrussMember>();
            _selectedNodes ??= new List<TrussNode>();
            _selectedMembers ??= new List<TrussMember>();
            _trussFactory = new TrussFactory();

            CreateNode(new Vector3(0,0,0));
            CreateNode(new Vector3(0,3,0));

            CreateMember(_nodes[0], _nodes[1]);

        }

        public  void Initalize()
        {
            //TODO: In the future I should initialize entire members from a saved file

        }
        
        public void CreateNode(Vector3 position)
        {
            TrussNode node = _trussFactory.CreateNode(position, this);
            AddNode(node);
        }

        public void CreateMember(TrussNode startNode, TrussNode endNode)
        {
            TrussMember member = _trussFactory.CreateMember(startNode, endNode, this);
            AddMember(member);
        }

        public void CreateMember(Vector3 start, Vector3 end)
        {
            TrussNode startNode = _trussFactory.CreateNode(start, this);
            TrussNode endNode = _trussFactory.CreateNode(end, this);
            AddNode(startNode);
            AddNode(endNode);
            CreateMember(startNode, endNode);
        }

        public void DeleteNode(TrussNode node)
        {
            RemoveNode(node);
            Destroy(node.gameObject);
        }

        public void DeleteMember(TrussMember member)
        {
            RemoveMember(member);
            Destroy(member.gameObject);
        }



        private void AddNode(TrussNode node)
        {
            if (_nodes.Contains(node))
                return;

            _nodes.Add(node);
        }

        private void RemoveNode(TrussNode node)
        {
            if (!_nodes.Contains(node))
                return;
            _nodes.Remove(node);
        }

        private void AddMember(TrussMember member)
        {
            if (_members.Contains(member))
                return;
            _members.Add(member);

        }

        private void RemoveMember(TrussMember memeber)
        {
            if (!_members.Contains(memeber))
                return;
            _members.Remove(memeber);
        }

        // Update is called once per frame
        private void Update()
        {
        }

        public void OnNodeClicked(TrussNode trussNode)
        {
            //if multiselect, add to selectedlists
        }

        public void OnNodeDragged(TrussNode trussNode)
        {
            
        }
    }
}