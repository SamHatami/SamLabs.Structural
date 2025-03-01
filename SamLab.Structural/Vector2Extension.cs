using System.Numerics;

namespace SamLab.Structural.Core;

public static class Vector2Extension
{
    public static float Cross(Vector2 a, Vector2 b)
    {
        return a.X * b.Y - a.Y * b.X;
    }
}