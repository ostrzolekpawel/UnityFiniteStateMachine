#if FSM_UNITASK_ENABLED
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Osiris.FSM
{
    public interface IFiniteStateMachineAsync<T> : IDisposable
    {
        void AddState(T stateType, IStateAsync<T> state);
        UniTask ChangeStateAsync(T stateType, CancellationToken externalToken = default);
        UniTask ForceChangeStateAsync(T stateType, CancellationToken externalToken = default);
        void Execute();
    }
}
#endif