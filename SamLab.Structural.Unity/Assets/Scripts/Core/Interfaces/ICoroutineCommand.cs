using System.Collections;

namespace Assets.Scripts.Core.Interfaces
{
    public interface ICoroutineCommand : ICommand
    {
        IEnumerator ExecuteCoroutine();
    }
}