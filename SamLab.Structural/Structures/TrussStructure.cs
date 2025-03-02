using SamLab.Structural.Core.Interfaces;
using SamLab.Structural.Core.StructuralElements;

namespace SamLab.Structural.Core.Structures;

public class TrussStructure : IStructure
{
    public required List<Node> Nodes { get; set; }
    public required List<Member> Members { get; set; }
    public required List<Force> ExternalForces { get; set; }
    public required List<Support> Supports { get; set; }


}