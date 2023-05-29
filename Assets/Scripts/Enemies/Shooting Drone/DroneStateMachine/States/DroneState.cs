using CharacterSM;

namespace DroneStateMachine.States
{
    public abstract class DroneState : BaseState
    {
        protected ShootingDrone character;
        protected IStateSwitcher stateSwitcher;

        protected DroneState(ShootingDrone character, IStateSwitcher stateSwitcher)
        {
            this.character = character;
            this.stateSwitcher = stateSwitcher;
        }

        public override void FixedUpdate(float time) 
        {
            character.MoveForward();
        }
    }
}
