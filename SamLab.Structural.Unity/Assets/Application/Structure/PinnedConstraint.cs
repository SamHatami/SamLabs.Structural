using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Application.Structure
{
    public class PinnedConstraint : MonoBehaviour
    {
        public TrussNode Node { get; set; }
        public TrussConstraint Constraint { get; set; }

        void Initialize(TrussNode node)
        {
            Node = node;

            SetDoFs();
        }

        private void SetDoFs()
        {
            Constraint.DegreeOfFreedom.Rx = Constraint.DegreeOfFreedom.Ry = Constraint.DegreeOfFreedom.Rz = false;
            Constraint.DegreeOfFreedom.Ux = Constraint.DegreeOfFreedom.Uy = Constraint.DegreeOfFreedom.Uz = true;

        }



    }
}
