using Assets.Application.Workspace.Interfaces;
using UnityEngine;

namespace Assets.Application.Workspace
{
    public class WorkAxis : IReferenceGeometry
    {
        public void Initialize()
        {
            throw new System.NotImplementedException();
        }

        public string Name { get; set; }
        public bool IsVisible { get; set; }
        public bool IsActive { get; set; }
    }
}