namespace Assets.Scripts.Workspace.Geometry.Interfaces
{
    public interface IReferenceGeometry
    {
        void Initialize();
        string Name { get; set; }
        bool IsVisible { get; set; }
        bool IsActive { get; set; }
    }
}
