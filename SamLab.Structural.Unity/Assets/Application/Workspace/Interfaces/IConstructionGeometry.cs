using UnityEditor;

namespace Assets.Application.Workspace.Interfaces
{
    public interface IConstructionGeometry
    {
        bool IsVisible { get; set; }
        bool IsActive { get; set; }

        IConstraint[] Constraints { get; set; }
    }
}