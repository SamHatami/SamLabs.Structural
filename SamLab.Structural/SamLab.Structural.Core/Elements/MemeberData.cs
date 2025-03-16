using System.Numerics;
using SamLab.Structural.Core.Analysis.Constraints;
using SamLab.Structural.Core.Elements.utilities;

namespace SamLab.Structural.Core.Elements
{
    public abstract class MemeberData
    {
        public NodeData Start { get; }
        public NodeData End { get; }
        public float Length { get; }
        public Vector2 Direction => Vector2.Normalize(End.Position - Start.Position);
        public int Id { get; set; }
        public Force AxialForce { get; protected set; }

        public MemeberData(NodeData start, NodeData end)
        {
            Start = start;
            End = end;
            Length = (end.Position - start.Position).Length();
            Id = GlobalIdHandler.GetNextMemberIndex();
        }

    
    }
}

