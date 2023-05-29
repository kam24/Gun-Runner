using PlayerMovement.Interfaces;

namespace PlayerMovement.States
{
    public class RunningState : ShootAbleState
    {
        public RunningState(PlayerCharacter character, PlayerStateMachine stateMachine) : base(character, stateMachine) 
        {
            InputHandler.SetUpKeyHandler(OnUpKey);
            InputHandler.SetDownKeyHandler(OnDownKey);
        }

        public override void Enter()
        {
            base.Enter();
            character.SetAnimationBool(character.AnimIDGrounded, true);
        }

        public override void Exit()
        {
            _startJump = false;
        }

        public override void FixedUpdate(float time)
        {
            if (character.IsGrounded() == false)
                SwitchState<InAirState>();
            else if (_startJump)
                Jump();
            else
                character.SetGroundedVelocity();
        }

        public void OnUpKey()
        {
            _startJump = true;
        }

        public void OnDownKey()
        {
            SwitchState<RollingState>();
        }
    }
}
