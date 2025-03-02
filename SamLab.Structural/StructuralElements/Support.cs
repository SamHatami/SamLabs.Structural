using System.Numerics;

namespace SamLab.Structural.Core.StructuralElements;

public class Support
{
    public Node Node { get; private set; }
    public BoundaryCondition BoundaryCondition { get; private set; }
    public Vector2 Orientation { get; private set; }
    public Force ReactionForce { get; private set; }

    public DoF DegreeOfFreedom { get; private set; } 
    public Support(Node node, BoundaryCondition boundaryCondition, Vector2 orientation)
    {
        Node = node;
        BoundaryCondition = boundaryCondition;
        Orientation = orientation;

        Initialize();
    }

    public void UpdateDegreeOfFreedom(DoF dof)
    {
        DegreeOfFreedom = dof;
    }

    public void UpdateReactionForce(Force force)
    {
        ReactionForce = force;
    }

    public void UpdateOrientation(Vector2 orientation)
    {
        Orientation = orientation;
    }

    private void Initialize()
    {
        switch (BoundaryCondition.Type)
        {
            case BoundaryConditionType.Fixed: //Not support
                break;

            case BoundaryConditionType.Free: //Not supproted -> Why have them, Sam?
                break;

            case BoundaryConditionType.Pinned:
                ReactionForce =
                    new Force(0, 0,
                        Node); //TODO: initalize a very small reactionforce to be able to check if the structure is stable?
                break;

            case BoundaryConditionType.Roller:
                ReactionForce =
                    new Force(0, 0,
                        Node); //TODO: initalize a very small reactionforce to be able to check if the structure is stable?
                break;

            default:
                break;
        }
    }
}