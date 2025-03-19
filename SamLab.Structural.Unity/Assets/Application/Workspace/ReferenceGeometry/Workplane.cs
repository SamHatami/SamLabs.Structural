using Assets.Application.Workspace.Interfaces;
using UnityEngine;

namespace Assets.Application.Workspace
{
    public class WorkPlane: MonoBehaviour, IReferenceGeometry
    {
        private void Start()
        {
            WorkspaceManager workspaceManager = FindFirstObjectByType<WorkspaceManager>();
            workspaceManager.Workplanes.Add(this);
            Initialize();
        }

        public void Initialize()
        {
            throw new System.NotImplementedException();
        }

        public string Name { get; set; }
        public bool IsVisible { get; set; }
        public bool IsActive { get; set; }
    }
}