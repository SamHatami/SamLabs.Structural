using System.Collections.Generic;
using SamLab.Structural.Core.Analysis.Constraints;
using SamLab.Structural.Core.Elements;

namespace SamLab.Structural.Core.Interfaces
{
    public interface IStructure
    {
        List<NodeData> Nodes { get; set;  }
        List<MemberData> Members { get; set; }
        List<Force> ExternalForces { get; set; }
        List<Support> Supports{ get; set; }

    }
}