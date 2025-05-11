using Core.Interfaces;
using UnityEngine;

namespace UI.ViewModels
{
    public class StructuralTreeGroupHeader : IStructuralElement
    {
        public string Name { get; set; }
        public GameObject SceneObject { get; set; } = null;
    }
}