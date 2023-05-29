using CharacterSM;

namespace PlayerMovement.States
{
    public abstract class PlayerState: BaseState
    {
        public virtual InputHandler InputHandler { get; private set; }

        protected PlayerCharacter character;
        protected PlayerStateMachine stateMachine;
        
        protected PlayerState(PlayerCharacter character, PlayerStateMachine stateMachine)
        {
            this.character = character;
            this.stateMachine = stateMachine;
            InputHandler = new();
        }
    }
}
