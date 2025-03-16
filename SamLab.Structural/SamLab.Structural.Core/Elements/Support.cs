using System.Numerics;
using SamLab.Structural.Core.Analysis.Constraints;

namespace SamLab.Structural.Core.Elements
{
    public class Support
    {
        public NodeData NodeData { get; private set; }
        public BoundaryCondition BoundaryCondition { get; private set; }
        public Vector2 Orientation { get; private set; }
        public Force ReactionForce { get; private set; }

        public DoF DegreeOfFreedom { get; private set; } 
        public Support(NodeData nodeData, BoundaryCondition boundaryCondition, Vector2 orientation)
        {
            NodeData = nodeData;
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
                            NodeData); //TODO: initalize a very small reactionforce to be able to check if the structure is stable?
                    DegreeOfFreedom = new DoF(true, true, false, false, false, false);
                    break;

                case BoundaryConditionType.Roller:
                    ReactionForce =
                        new Force(0, 0,
                            NodeData); //TODO: initalize a very small reactionforce to be able to check if the structure is stable?
                    DegreeOfFreedom = new DoF(false, true, false, false, false, false);
                    break;

                default:
                    break;
            }
        }
    }
}