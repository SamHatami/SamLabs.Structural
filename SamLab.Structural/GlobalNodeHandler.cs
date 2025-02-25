namespace SamLab.Structural.Core;

public class GlobalNodeHandler
{
    private static int _currentIndex = 0;

    public static int GetNextIndex()
    {
        return Interlocked.Increment(ref _currentIndex);
    }
}