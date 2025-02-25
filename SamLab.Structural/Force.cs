using System.Numerics;

namespace SamLab.Structural.Core;

public struct Force
{
    public float Magnitude { get; private set; }
    public Vector2 Direction { get; private set; }
    public Node InsertionNode { get; private set; }

    public Force(float magnitude, Vector2 direction, Node insertionNode)
    {
        Magnitude = magnitude;
        Direction = direction;
        InsertionNode = insertionNode;
    }

    public void UpdateDirection(Vector2 direction)
    {
        Direction = direction;
    }

    public void UpdateMagnitude(float magnitude)
    {
        Magnitude = magnitude;
    }

    public void MoveToNode(Node newNode)
    {
        InsertionNode = newNode;
    }
}