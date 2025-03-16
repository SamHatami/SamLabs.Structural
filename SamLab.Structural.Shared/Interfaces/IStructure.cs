using System.Collections.Generic;
using SamLab.Structural.Core.StructuralElements;

namespace SamLab.Structural.Core.Interfaces
{
    public interface IStructure
    {
        List<Node> Nodes { get; set;  }
        List<Member> Members { get; set; }
        List<Force> ExternalForces { get; set; }
        List<Support> Supports{ get; set; }

    }
}