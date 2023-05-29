using PlayerMovement.Interfaces;

namespace PlayerMovement.States
{
    public class InAirState : VerticalState
    {
        public InAirState(PlayerCharacter character, PlayerStateMachine stateMachine) : base(character, stateMachine) 
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
                stateMachine.SwitchState<RunningState>();
        }

        public void OnDownKey()
        {
            character.SetFastFallDownVelocity();
            stateMachine.SwitchState<RollingState>();
        }
    }
}
