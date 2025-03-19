using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SamLab.Structural.Core.Analysis.Constraints;

namespace Assets.Application.Structure
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
