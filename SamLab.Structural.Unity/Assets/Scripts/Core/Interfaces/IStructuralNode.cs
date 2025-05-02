using System.Collections.Generic;
using SamLab.Structural.Core.Elements;
using Structure.Base;
using Structure.Managers;

namespace Core.Interfaces
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