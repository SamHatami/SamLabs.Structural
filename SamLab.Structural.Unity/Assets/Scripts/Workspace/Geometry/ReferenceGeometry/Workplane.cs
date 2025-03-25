using Assets.Scripts.Workspace.Geometry.Interfaces;
using Assets.Scripts.Workspace.Managers;
using UnityEngine;

namespace Assets.Scripts.Workspace.Geometry.ReferenceGeometry
{
    public class WorkPlane : MonoBehaviour, IReferenceGeometry, IPlane
    {
        [SerializeField] private string _name;
        [SerializeField] private bool _isVisible;
        [SerializeField] private bool _isActive;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public bool IsVisible
        {
            get => _isVisible;
            set => _isVisible = value;
        }

        public bool IsActive
        {
            get => _isActive;
            set => _isActive = value;
        }

        public Vector3 Direction1
        {
            get => transform.right;
            set => transform.right = value;
        }

        public Vector3 Direction2
        {
            get => transform.up;
            set => transform.up = value;
        }

        public Vector3 Normal
        {
            get => transform.forward;
            set => transform.forward = value;
        }

        public Vector3 Origo
        {
            get => transform.position;
            set => transform.position = value;
        }

        public Quaternion Rotation
        {
            get => transform.rotation;
            set => transform.rotation = value;
        }

        private void Start()
        {
            Rotation = transform.rotation;
            Origo = transform.position;
            Normal = transform.forward;
            Direction1 = transform.right;
            Direction2 = transform.up;
            var workspaceManager = FindFirstObjectByType<WorkspaceManager>();
            workspaceManager.Workplanes.Add(this);
            Initialize();
        }

        public void Initialize()
        {
            Normal = transform.forward;
        }
    }
}