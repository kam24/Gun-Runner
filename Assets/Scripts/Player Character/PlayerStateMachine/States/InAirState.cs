using PlayerMovement.Interfaces;

namespace PlayerMovement.States
{
    public class InAirState : VerticalState
    {
        public InAirState(PlayerCharacter character, StateMachine stateMachine) : base(character, stateMachine) 
        {
            InputHandler.SetDownKeyHandler(OnDownKey);
        }

        public override void Enter()
        {
            base.Enter();
            character.SetAnimationBool(character.AnimIDGrounded, false);
        }

        public override void FixedUpdate(float time)
        {
            if (character.IsGrounded())
                stateMachine.ChangeBaseState(character.RunningState);
        }

        public void OnDownKey()
        {
            character.SetFastFallDownVelocity();
            stateMachine.ChangeBaseState(character.RollingState);
        }
    }
}
