using System.Collections.Generic;
using Assets.Scripts.Structure.Base;
using Assets.Scripts.Structure.Managers;
using SamLab.Structural.Core.Elements;

namespace Assets.Scripts.Core.Interfaces
{
    public interface IStructuralNode
    {
        NodeData NodeData { get; set; }
        List<TrussStructure> ParentStructures { get; }
        List<TrussElement> ConnectedElements { get; }
        bool IsShared { get; set; }

        void AddParentStructure(TrussStructure structure);
        void RemoveParentStructure(TrussStructure structure);
    }
}