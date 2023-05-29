using DroneStateMachine.States;

namespace DroneStateMachine
{
    public interface IStateSwitcher
    {
        void SwitchState<T>() where T : State;
    }
}
