using SamLab.Structural.Core.StructuralElements;

namespace SamLab.Structural.Core.Interfaces;

public interface IStructure
{
    public List<Node> Nodes { get; set;  }
    public List<Member> Members { get; set; }
    public List<Force> ExternalForces { get; set; }
    public List<Support> Supports{ get; set; }

}