using System.Linq.Expressions;
using Core.Interfaces;
using SamLab.Structural.Core.Analysis.Constraints;
using UnityEngine;

namespace Structure.Base.Constraints
{
    public class PinnedSupport : MonoBehaviour, IConstraint
    {
        [SerializeField] private TrussNode _attachedNode;

        public string Name
        {
            get => name;
            set => name = value;
        }
        public TrussNode AttachedNode
        {
            get => _attachedNode; 
            set => _attachedNode = value;
        }

        private Vector3 _currentPosition;
        public DoF DegreeOfFreedoms { get; set; }

        private void Awake()
        {
            DegreeOfFreedoms = new DoF(true,true,true,false,false,false);
            _currentPosition = transform.position;
        }
        private void Update()
        {
            if (_attachedNode && _currentPosition != _attachedNode.transform.position)
                _currentPosition = transform.position = _attachedNode.transform.position;
        }
    }
}