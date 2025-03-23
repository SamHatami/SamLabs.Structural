using Assets.Scripts.Workspace.Geometry.Interfaces;

namespace Assets.Scripts.Workspace.Geometry.ReferenceGeometry
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