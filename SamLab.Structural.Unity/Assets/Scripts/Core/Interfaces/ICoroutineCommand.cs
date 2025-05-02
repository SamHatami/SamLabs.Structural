using System.Collections;

namespace Core.Interfaces
{
    public interface ICoroutineCommand : ICommand
    {
        IEnumerator ExecuteCoroutine();
    }
}