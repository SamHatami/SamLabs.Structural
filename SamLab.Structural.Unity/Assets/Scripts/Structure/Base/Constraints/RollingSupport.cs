using SamLab.Structural.Core.Analysis.Constraints;
using UnityEngine;

namespace Structure.Base.Constraints
{
    public class RollingSupport : MonoBehaviour, IConstraint
    {
        [SerializeField] private TrussNode _attachedNode;

        [SerializeField] private bool ux;
        [SerializeField] private bool uy;
        [SerializeField] private bool uz;

        public bool Ux
        {
            get => ux;
            set
            {
                ux = value;
                uy = uz = !ux;
            }
        }

        public bool Uy
        {
            get => uy;
            set
            {
                uy = value;
                ux = uz = !uy;
            }
        }

        public bool Uz
        {
            get => uz;
            set
            {
                uz = value;
                ux = uy = !uz;
            }
        }

        public DoF DegreeOfFreedoms { get; set; }

        private Vector3 _currentPosition;

        private void Awake()
        {
            DegreeOfFreedoms = new DoF(false, true, false, false, false, false);
        }

        private void Update()
        {
            if (_attachedNode && _currentPosition != _attachedNode.transform.position)
                _currentPosition = transform.position = _attachedNode.transform.position;
        }
    }
}