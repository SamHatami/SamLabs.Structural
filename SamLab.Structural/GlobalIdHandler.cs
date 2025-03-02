namespace SamLab.Structural.Core;

public class GlobalIdHandler //TODO: This is a temporary solution, it will be replaced with a more robust solution in the future.
{
    private static int _currentNodeIndex = 0;
    private static int _currentMemberIndex = 0;

    public static int GetNextNodeIndex()
    {
        return Interlocked.Increment(ref _currentNodeIndex);
    }

    public static int GetNextMemberIndex()
    {
        return Interlocked.Increment(ref _currentMemberIndex);
    }
}