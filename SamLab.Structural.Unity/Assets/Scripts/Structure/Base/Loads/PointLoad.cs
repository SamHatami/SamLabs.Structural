using System;
using Structure.Interfaces;
using UnityEngine;
using Workspace.Camera;

namespace Structure.Base.Loads
{
    public class PointLoad : MonoBehaviour, ILoad
    {
        [SerializeField] private Vector3 _force = Vector3.zero;

        [SerializeField] private TrussNode _attachedNode;

        private Vector3 _currentPosition;

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

        private Vector3 _lastForceValues = Vector3.zero;

        private void Awake()
        {
            _currentPosition = transform.position;
            _lastForceValues = Force;
        }

        private void Update()
        {
            if (_attachedNode != null && _currentPosition != _attachedNode.transform.position)
                _currentPosition = transform.position = _attachedNode.transform.position;


            if (_lastForceValues != Force)
            {
                float x = Mathf.Clamp(Force.x, -1.0f, 1.0f);
                float y = Mathf.Clamp(Force.y, -1.0f, 1.0f);
                float z = Mathf.Clamp(Force.z, -1.0f, 1.0f);
                
                // The prefab arrow points in 0,1,0: so whenever the forces are 0, the direction will be 0,1,0
                transform.rotation = Quaternion.FromToRotation(Vector3.up, new Vector3(x, y, z));

                _lastForceValues = Force;
            }
        }
    }
}