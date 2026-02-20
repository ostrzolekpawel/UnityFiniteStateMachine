namespace Osiris.FSM
{
    public interface IFinishStateMachine<T>
    {
        void AddState(T stateType, IState<T> state);
        void Execute();
        void ChangeState(T stateType);
        void ForceChangeState(T stateType);
    }
}