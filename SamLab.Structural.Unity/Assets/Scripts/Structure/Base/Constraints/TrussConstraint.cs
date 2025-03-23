using SamLab.Structural.Core.Analysis.Constraints;

namespace Assets.Scripts.Structure.Base.Constraints
{
    public class TrussConstraint
    {
        public DoF DegreeOfFreedom;
        void Start()
        {
            // Create a new truss constraint
            TrussConstraint trussConstraint = new TrussConstraint();
            // Set the degree of freedom
            // Get the degree of freedom
            DoF degreeOfFreedom = trussConstraint.DegreeOfFreedom;
        }
    }
}
