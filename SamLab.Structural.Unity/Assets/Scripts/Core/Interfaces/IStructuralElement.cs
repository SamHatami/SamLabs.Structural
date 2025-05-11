using UnityEngine;

namespace Core.Interfaces
{
    public interface IStructuralElement
    {
        string Name { get; set; }
        
        GameObject SceneObject { get; set; }
    }
}