using SamLab.Structural.Core.Analysis.Constraints;

namespace Structure.Base.Constraints
{
    public interface IConstraint
    {
        DoF DegreeOfFreedoms { get; set; }
    }
}