#if FSM_UNITASK_ENABLED
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Osiris.FSM
{
    public interface IStateAsync<T>
    {
        UniTask EnterAsync(CancellationToken token);
        UniTask ExitAsync(CancellationToken token);
        T Execute();
        bool CanChange(T nextState);
    }
}
#endif