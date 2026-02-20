using System;
using System.Collections.Generic;

namespace Osiris.FSM
{
    public abstract class FiniteStateMachine<T> : IFinishStateMachine<T>
    {
        protected readonly Dictionary<T, IState<T>> _states = new Dictionary<T, IState<T>>();
        protected IState<T> _currentState;
        protected T _currentStateType;

        public void AddState(T stateType, IState<T> state)
        {
            _states[stateType] = state;
        }

        public virtual void ChangeState(T stateType)
        {
            if (!_states.TryGetValue(stateType, out var nextState))
            {
                throw new ArgumentException($"Invalid state type: {stateType}", nameof(stateType));
            }

            if (_currentState != null && !_currentState.CanChange(stateType))
            {
                return;
            }

            _currentState?.Exit();
            _currentState = nextState;
            _currentStateType = stateType;
            _currentState?.Enter();
        }

        public virtual void ForceChangeState(T stateType)
        {
            if (!_states.TryGetValue(stateType, out var nextState))
            {
                throw new ArgumentException($"Invalid state type: {stateType}", nameof(stateType));
            }

            _currentState?.Exit();
            _currentState = nextState;
            _currentStateType = stateType;
            _currentState?.Enter();
        }

        public void Execute()
        {
            if (_currentState != null)
            {
                T nextState = _currentState.Execute();
                if (!EqualityComparer<T>.Default.Equals(nextState, _currentStateType))
                {
                    ChangeState(nextState);
                }
            }
        }
    }
}