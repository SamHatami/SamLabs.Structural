using SamLab.Structural.Core.Analysis.Constraints;

namespace Core.Interfaces
{
    public interface IConstraint: IStructuralElement
    {
        DoF DegreeOfFreedoms { get; set; }
    }
}