using PlayerMovement.Interfaces;
using System;

namespace PlayerMovement.States
{
    public class RollingState : VerticalState
    {
        public RollingState(PlayerCharacter character, PlayerStateMachine stateMachine) : base(character, stateMachine) 
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
                stateMachine.SwitchState<InAirState>();
            }
        }

        public void OnUpKey()
        {
            _startJump = true;
        }

        public void OnEnding()
        {
            if (character.IsGrounded())
                stateMachine.SwitchState<RunningState>();
            else
                stateMachine.SwitchState<InAirState>();
        }
    }
}
