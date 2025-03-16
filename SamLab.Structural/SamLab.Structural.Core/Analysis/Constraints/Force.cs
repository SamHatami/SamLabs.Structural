using System;
using System.Numerics;
using SamLab.Structural.Core.Analysis.Utilities;
using SamLab.Structural.Core.Elements;

namespace SamLab.Structural.Core.Analysis.Constraints
{
    public class Force
    {
        public float X { get; private set; }
        public float Y { get; private set; }

        public float Magnitude => (float)Math.Sqrt(X * X + Y * Y);
        public Vector2 Direction => Vector2.Normalize(new Vector2(X, Y));

        public NodeData InsertionNodeData { get; private set; }

        public Force(float x, float y, NodeData insertionNodeData)
        {
            X = x;
            Y = y;
            InsertionNodeData = insertionNodeData;
        }

        public Force(float magnitude, float angleRadians, NodeData insertionNodeData, bool fromPolar)
        {
            X = magnitude * (float)Math.Cos(angleRadians);
            Y = magnitude * (float)Math.Sin(angleRadians);
            InsertionNodeData = insertionNodeData;
        }

        public void UpdateComponents(float x, float y)
        {
            X = x;
            Y = y;
        }

        public void UpdateMagnitudeAndAngle(float magnitude, float angleRadians)
        {
            X = magnitude * (float)Math.Cos(angleRadians);
            Y = magnitude * (float)Math.Sin(angleRadians);
        }

        public void MoveToNode(NodeData newNodeData)
        {
            InsertionNodeData = newNodeData;
        }

        public float CalculateMoment(Vector2 pivotPoint) //TODO: Handle this by a StructuralAnalysis class
        {
            Vector2 r = InsertionNodeData.Position - pivotPoint;
            return Vector2Extension.Cross(r, new Vector2(X, Y));

        }

        public float CalculateMoment(NodeData pivotNodeData) //TODO: Handle this by a StructuralAnalysis class
        {
            return CalculateMoment(pivotNodeData.Position);
        }
    }
}
