namespace SamLab.Structural.Core.StructuralElements;

public abstract class Member
{
    public Node Start { get; }
    public Node End { get; }
    public float Length { get; }

    public float AxialForce { get; protected set; }

    public Member(Node start, Node end)
    {
        Start = start;
        End = end;
        Length = (end.Position - start.Position).Length();
    }

    public abstract void CalculateForces();
}

