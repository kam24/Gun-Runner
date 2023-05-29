using PlayerMovement.Interfaces;

namespace PlayerMovement.States
{
    public class RollingState : VerticalState
    {
        public RollingState(PlayerCharacter character, StateMachine stateMachine) : base(character, stateMachine) 
        {
            InputHandler.SetUpKeyHandler(OnUpKey);
        }

        public override void Enter()
        {
            base.Enter();
            character.SetAnimationTrigger(character.AnimIDRoll);
        }

        public override void Exit()
        {
            character.SetNormalHeight();
            _startJump = false;
        }

        public override void FixedUpdate(float time)
        {
            if (character.IsGrounded() && _startJump) 
            {
                Jump();
                stateMachine.ChangeBaseState(character.InAirState);
            }
        }

        public void OnUpKey()
        {
            _startJump = true;
        }

        public void OnEnding()
        {
            BaseState newState = character.IsGrounded() ? character.RunningState : character.InAirState;
            stateMachine.ChangeBaseState(newState);
        }
    }
}
