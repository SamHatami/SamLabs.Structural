using SamLab.Structural.Core.Structures;

namespace SamLab.Structural.Core;

public static class TrussStructureSolver
{
    public static int NrOfPinnedSupports(this TrussStructure structure)
    {
        return structure.Supports.Count(s => s.BoundaryCondition.Type == BoundaryConditionType.Pinned);
    }

    public static int NrOfRollerSupports(this TrussStructure structure)
    {
        return structure.Supports.Count(s => s.BoundaryCondition.Type == BoundaryConditionType.Roller);
    }

    public static int TotalReactionComponents(this TrussStructure structure)
    {
        return structure.NrOfPinnedSupports() * 2 + structure.NrOfRollerSupports();
    }

    public static bool IsStaticallyDeterminate(this TrussStructure structure)
    {
        var members = structure.Members.Count;
        var joints = structure.Nodes.Count;
        var reactions = structure.TotalReactionComponents();

        return members + reactions == 2 * joints;
    }

    public static double[,] BuildGlobalMatrix(this TrussStructure structure, Dictionary<int, int> nodeIndexMap)
    {
        var nrOfRows = structure.Nodes.Count * 2;
        var nrOfColumns = structure.Members.Count + structure.TotalReactionComponents();

        var matrix = new double[nrOfRows, nrOfColumns];

        if (nodeIndexMap == null) nodeIndexMap = structure.BuildNodeIndexMap();

        for (var i = 0; i < structure.Members.Count; i++)
        {
            var startNodeIndex = nodeIndexMap[structure.Members[i].Start.Id];
            var endNodeIndex = nodeIndexMap[structure.Members[i].End.Id];

            var member = structure.Members[i];

            matrix[2 * startNodeIndex, i] = member.Direction.X;
            matrix[2 * startNodeIndex + 1, i] = member.Direction.Y;
            matrix[2 * endNodeIndex, i] = -member.Direction.X;
            matrix[2 * endNodeIndex + 1, i] = -member.Direction.Y;
        }

        var reactionIndex = structure.Members.Count;

        for (var i = 0; i < structure.Supports.Count; i++)
        {
            var supportNodeIndex = nodeIndexMap[structure.Supports[i].Node.Id];

            if (structure.Supports[i].BoundaryCondition.Type == BoundaryConditionType.Pinned)
            {
                matrix[2 * supportNodeIndex, reactionIndex] = 1;
                matrix[2 * supportNodeIndex + 1, reactionIndex] = 1;
                reactionIndex += 2;
            }
            else if (structure.Supports[i].BoundaryCondition.Type ==
                     BoundaryConditionType.Roller) //Should be allowed to set x or y to be free, or angled?
            {
                //TODO: Improve next pass.
                if (structure.Supports[i].DegreeOfFreedom.Ux)
                    matrix[2 * supportNodeIndex, reactionIndex] = 1;
                if (structure.Supports[i].DegreeOfFreedom.Uy)
                    matrix[2 * supportNodeIndex + 1, reactionIndex] = 1;
                reactionIndex++;
            }
        }

        return matrix;
    }

    public static double[] BuildGlobalForceVector(this TrussStructure structure, Dictionary<int, int> nodeIndexMap)
    {
        if (nodeIndexMap == null) nodeIndexMap = structure.BuildNodeIndexMap();

        var nrOfRows = structure.Nodes.Count * 2;
        var vector = new double[nrOfRows];
        for (var i = 0; i < structure.ExternalForces.Count; i++)
        {
            var nodeIndex = nodeIndexMap[structure.ExternalForces[i].InsertionNode.Id];
            vector[2 * nodeIndex] = structure.ExternalForces[i].X;
            vector[2 * nodeIndex + 1] = structure.ExternalForces[i].Y;
        }

        return vector;
    }

    private static Dictionary<int, int> BuildNodeIndexMap(this TrussStructure structure)
    {
        var nodeIndexMap = new Dictionary<int, int>();
        for (var i = 0; i < structure.Nodes.Count; i++) nodeIndexMap.Add(structure.Nodes[i].Id, i);
        return nodeIndexMap;
    }

    public static double[] Solve(this TrussStructure structure)
    {
        var nodeIndexMap = BuildNodeIndexMap(structure);
        for (var i = 0; i < structure.Nodes.Count; i++) nodeIndexMap.Add(structure.Nodes[i].Id, i);
        var globalMatrix = structure.BuildGlobalMatrix(nodeIndexMap);
        var globalForceVector = structure.BuildGlobalForceVector(nodeIndexMap);
        return MathNet.Numerics.LinearAlgebra.Double.Matrix.Build.DenseOfArray(globalMatrix)
            .Solve(MathNet.Numerics.LinearAlgebra.Double.Vector.Build.DenseOfArray(globalForceVector)).ToArray();
    }
}