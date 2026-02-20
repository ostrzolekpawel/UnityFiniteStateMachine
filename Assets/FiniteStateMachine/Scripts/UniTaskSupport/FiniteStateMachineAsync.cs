#if FSM_UNITASK_ENABLED
using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Osiris.FSM
{
    public abstract class FiniteStateMachineAsync<T> : IFiniteStateMachineAsync<T>
    {
        protected readonly Dictionary<T, IStateAsync<T>> _states = new Dictionary<T, IStateAsync<T>>();
        private readonly SemaphoreSlim _transitionLock = new SemaphoreSlim(1, 1);

        protected IStateAsync<T> _currentState;
        protected T _currentStateType;
        private CancellationTokenSource _stateCts;

        public virtual void AddState(T stateType, IStateAsync<T> state)
        {
            _states[stateType] = state;
        }

        public virtual async UniTask ChangeStateAsync(T stateType, CancellationToken externalToken = default)
        {
            if (!_states.TryGetValue(stateType, out var nextState))
                throw new ArgumentException($"Invalid state type: {stateType}");

            await _transitionLock.WaitAsync(externalToken);

            try
            {
                if (_currentState != null && !_currentState.CanChange(stateType))
                    return;

                _stateCts?.Cancel();
                _stateCts?.Dispose();

                _stateCts = CancellationTokenSource.CreateLinkedTokenSource(externalToken);
                var token = _stateCts.Token;

                if (_currentState != null)
                {
                    try
                    {
                        await _currentState.ExitAsync(token);
                    }
                    catch (OperationCanceledException) when (token.IsCancellationRequested) { }
                }

                _currentState = nextState;
                _currentStateType = stateType;

                await _currentState.EnterAsync(token);
            }
            finally
            {
                _transitionLock.Release();
            }
        }

        public virtual async UniTask ForceChangeStateAsync(
            T stateType,
            CancellationToken externalToken = default)
        {
            if (!_states.TryGetValue(stateType, out var nextState))
                throw new ArgumentException($"Invalid state type: {stateType}");

            await _transitionLock.WaitAsync(externalToken);

            try
            {
                _stateCts?.Cancel();
                _stateCts?.Dispose();

                _stateCts = CancellationTokenSource.CreateLinkedTokenSource(externalToken);
                var token = _stateCts.Token;

                if (_currentState != null)
                {
                    try
                    {
                        await _currentState.ExitAsync(token);
                    }
                    catch (OperationCanceledException) when (token.IsCancellationRequested) { }
                }

                _currentState = nextState;
                _currentStateType = stateType;

                await _currentState.EnterAsync(token);
            }
            finally
            {
                _transitionLock.Release();
            }
        }

        public void Execute()
        {
            if (_currentState == null)
                return;

            var nextState = _currentState.Execute();

            if (!EqualityComparer<T>.Default.Equals(nextState, _currentStateType))
            {
                ChangeStateAsync(nextState).Forget(HandleTransitionException);
            }
        }

        protected virtual void HandleTransitionException(Exception ex)
        {
            UnityEngine.Debug.LogException(ex);
        }

        public void Dispose()
        {
            _stateCts?.Cancel();
            _stateCts?.Dispose();
            _transitionLock.Dispose();
        }
    }
}
#endif