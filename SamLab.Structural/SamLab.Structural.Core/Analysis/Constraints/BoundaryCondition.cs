using SamLab.Structural.Core.Elements;

namespace SamLab.Structural.Core.Analysis.Constraints
{
    public class BoundaryCondition
    {
        public NodeData NodeData { get; private set; }
        public BoundaryConditionType Type { get; private set; }

        public BoundaryCondition(NodeData nodeData, BoundaryConditionType type)
        {
            NodeData = nodeData;
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
}