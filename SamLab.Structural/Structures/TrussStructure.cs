using SamLab.Structural.Core.Interfaces;
using SamLab.Structural.Core.StructuralElements;

namespace SamLab.Structural.Core.Structures;

public class TrussStructure : IStructure
{
    public List<Node> Nodes { get; }
    public List<Member> Members { get; }
    public List<Force> ExternalForces { get; }
    public List<Support> Supports { get; }


}