using Assets.Scripts.Structure.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Structure.Base.Loads
{
    public class PointLoad : MonoBehaviour, ILoad
    {
        [SerializeField] private Vector3 _force = Vector3.zero;

        [SerializeField] private TrussNode _attachedNode;
        public TrussNode AttachedNode
        {
            get => _attachedNode;
            set => _attachedNode = value;
        }

        public Vector3 Force
        {
            get => _force;
            set => _force = value;
        }


    }
}