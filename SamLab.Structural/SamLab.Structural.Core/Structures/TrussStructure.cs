using System.Collections.Generic;
using SamLab.Structural.Core.Analysis.Constraints;
using SamLab.Structural.Core.Elements;
using SamLab.Structural.Core.Interfaces;

namespace SamLab.Structural.Core.Structures
{
    public class TrussStructure : IStructure
    {
        public List<NodeData> Nodes { get; set; }
        public List<MemberData> Members { get; set; }
        public List<Force> ExternalForces { get; set; }
        public List<Support> Supports { get; set; }


    }
}