using System.Numerics;
using SamLab.Structural.Core.StructuralElements;

namespace SamLab.Structural.Core
{
    public struct Node
    {
        public Vector2 Position { get; private set; }
        public BoundaryCondition BoundaryCondition { get; } = BoundaryCondition.Pinned;
        public int Id { get; set; }

        public Vector2 AppliedForce;
        public Vector2 Displacement;

        private List<Member> Members;
        public Node(Vector2 position, BoundaryCondition boundaryCondition)
        {
            Position = position;
            BoundaryCondition = boundaryCondition;
            Id = GlobalNodeHandler.GetNextIndex(); 
            AppliedForce = Vector2.Zero;
            Displacement = Vector2.Zero;
        }

        public void SetForce(Vector2 force)
        {
            AppliedForce = force;
        }

        public void SetPosition(Vector2 position)
        {
            Displacement = position - Position;
            Position = position;
        }

        public void Reset()
        {
            AppliedForce = Vector2.Zero;
            Displacement = Vector2.Zero;
        }

        public void AddMember(Member member)
        {
            if(Members.Contains(member))
                return;

            Members.Add(member);
        }
    }
}
