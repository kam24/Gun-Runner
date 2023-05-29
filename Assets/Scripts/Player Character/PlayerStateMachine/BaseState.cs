namespace PlayerMovement.States
{
    public abstract class BaseState
    {
        public virtual InputHandler InputHandler { get; private set; }

        protected PlayerCharacter character;
        protected StateMachine stateMachine;
        
        protected BaseState(PlayerCharacter character, StateMachine stateMachine)
        {
            this.character = character;
            this.stateMachine = stateMachine;
            InputHandler = new();
        }

        public virtual void Enter()
        {

        }

        public virtual void Exit()
        {

        }

        public virtual void FixedUpdate(float time)
        {

        }

    }
}
