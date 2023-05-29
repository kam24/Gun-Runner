namespace CharacterSM
{
    public interface IStateSwitcher
    {
        public abstract void SwitchState<T>() where T : BaseState;
    }
}