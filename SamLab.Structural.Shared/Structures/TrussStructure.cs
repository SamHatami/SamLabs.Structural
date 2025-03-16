using System.Collections.Generic;
using SamLab.Structural.Core.Interfaces;
using SamLab.Structural.Core.StructuralElements;

namespace SamLab.Structural.Core.Structures
{
    public class TrussStructure : IStructure
    {
        public List<Node> Nodes { get; set; }
        public List<Member> Members { get; set; }
        public List<Force> ExternalForces { get; set; }
        public List<Support> Supports { get; set; }


    }
}