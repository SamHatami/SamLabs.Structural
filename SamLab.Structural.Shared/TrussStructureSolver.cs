using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SamLab.Structural.Core.StructuralElements;
using SamLab.Structural.Core.Structures;

namespace SamLab.Structural.Core
{
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

            foreach (var support in structure.Supports)
            {
                var supportNodeIndex = nodeIndexMap[support.Node.Id];

                //The enum here is redundant, but kept for clarity.
                //user should be able to set the degree of freedom for each support and the global matrix should be built accordingly.
                //Pre-defined support types are just a convenience.
                //user will be able to set DoF for each support in the future in the GUI
                if (support.BoundaryCondition.Type == BoundaryConditionType.Pinned) 
                {
                    matrix[2 * supportNodeIndex, reactionIndex] = 1;
                    matrix[2 * supportNodeIndex + 1, reactionIndex + 1] = 1;
                    reactionIndex += 2;
                }
                else if (support.BoundaryCondition.Type ==
                         BoundaryConditionType.Roller) //Should be allowed to set x or y to be free, or angled?
                {
                    //TODO: Improve next pass.
                    if (support.DegreeOfFreedom.Ux)
                        matrix[2 * supportNodeIndex, reactionIndex] = 1;
                    else if (support.DegreeOfFreedom.Uy)
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
            //Signage conventions:
            //Memberforces => Negative in tension / Positive in compression
            //Reactionforces should be reversed to give correct physical interpretation. 

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var nodeIndexMap = BuildNodeIndexMap(structure);
            var globalMatrix = structure.BuildGlobalMatrix(nodeIndexMap);
            var globalForceVector = structure.BuildGlobalForceVector(nodeIndexMap);
            sw.Stop();

            return MathNet.Numerics.LinearAlgebra.Double.Matrix.Build.DenseOfArray(globalMatrix)
                .Solve(MathNet.Numerics.LinearAlgebra.Double.Vector.Build.DenseOfArray(globalForceVector)).ToArray();
        }

        //Claude made this very nice printing method
        public static void PrintMatrixAndVector(double[,] matrix, double[] vector)
        {
            var rows = matrix.GetLength(0);
            var cols = matrix.GetLength(1);

            Trace.WriteLine("Global Matrix:");
            Trace.WriteLine("-------------");

            // Format each cell with 4 decimal places
            string[,] formattedCells = new string[rows, cols];
            for (var i = 0; i < rows; i++)
            for (var j = 0; j < cols; j++)
                formattedCells[i, j] = matrix[i, j].ToString("F4");

            // Calculate the width for each column
            var colWidths = new int[cols];
            for (var j = 0; j < cols; j++)
            {
                var maxWidth = $"Col{j}".Length;
                for (var i = 0; i < rows; i++) maxWidth = Math.Max(maxWidth, formattedCells[i, j].Length);
                colWidths[j] = maxWidth;
            }

            // Print column headers
            var headerRow = new StringBuilder("       ");
            for (var j = 0; j < cols; j++) headerRow.Append($"Col{j}".PadRight(colWidths[j] + 2));
            headerRow.Append(" | Force");
            Trace.WriteLine(headerRow.ToString());

            // Print separator line
            var separatorRow = new StringBuilder("       ");
            for (var j = 0; j < cols; j++) separatorRow.Append(new string('-', colWidths[j] + 1) + " ");
            separatorRow.Append("-|-------");
            Trace.WriteLine(separatorRow.ToString());

            // Print each row of the matrix along with the corresponding force value
            for (var i = 0; i < rows; i++)
            {
                var dataRow = new StringBuilder($"Row{i}: ");
                for (var j = 0; j < cols; j++) dataRow.Append(formattedCells[i, j].PadRight(colWidths[j] + 2));
                dataRow.Append($" | {vector[i]:F2}");
                Trace.WriteLine(dataRow.ToString());
            }

            Trace.WriteLine("");
        }
    }
}