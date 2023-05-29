namespace DroneStateMachine.States
{
    public abstract class State
    {
        protected ShootingDrone character;
        protected IStateSwitcher stateSwitcher;

        protected State(ShootingDrone character, IStateSwitcher stateSwitcher)
        {
            this.character = character;
            this.stateSwitcher = stateSwitcher;
        }

        public virtual void Enter() { }

        public virtual void Exit() { }

        public virtual void FixedUpdate(float time) 
        {
            character.MoveForward();
        }
    }
}
