namespace SamLab.Structural.Core;

public class BoundaryCondition
{
    public Node Node { get; private set; }
    public BoundaryConditionType Type { get; private set; }

    public BoundaryCondition(Node node, BoundaryConditionType type)
    {
        Node = node;
        Type = type;
    }
}

public enum BoundaryConditionType
{
    Fixed,
    Free,
    Pinned,
    Roller
}