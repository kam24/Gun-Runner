using PlayerMovement.Interfaces;
using UnityEngine;
using static PlayerCharacter;

namespace PlayerMovement.States
{
    public class WallRunState : ShootAbleState
    {
        public WallRunState(PlayerCharacter character, PlayerStateMachine stateMachine) : base(character, stateMachine) 
        {
            InputHandler.SetLeftRightKeyHandler(OnLeftRightKey);
            InputHandler.SetDownKeyHandler(OnDownKey);
        }

        public override void Enter()
        {
            base.Enter();
            character.SetAnimationBool(character.AnimIDWallRun, true);
            character.SetAnimationBool(character.AnimIDGrounded, false);
            character.SetZeroGravity();
        }

        public override void Exit()
        {
            character.WallRunning = WallRun.None;
            character.SetAnimationBool(character.AnimIDWallRun, false);
            character.SetNormalGravity();
        }

        public override void FixedUpdate(float time)
        {
            float positionY = character.transform.position.y;
            if (positionY != character.WallRunHeight)
            {
                float offsetY = Mathf.MoveTowards(positionY, character.WallRunHeight, time * character.WallRunFallDownSpeed) - positionY;
                character.Move(0, offsetY);
            }

            if (character.CheckRunningWall() == false)
                OnDownKey();
        }

        public override void OnLeftRightKey()
        {            
            if ((int)character.LastStrafe == (int)character.WallRunning)
            {
                base.OnLeftRightKey();
                stateMachine.SwitchState<InAirAfterWallRunState>();
                character.JumpForStrafe(character.LastStrafe);
            }
        }

        public void OnDownKey()
        {
            character.LastStrafe = Strafe.None;
            stateMachine.SwitchState<RollingState>();
            DoStrafe();
        }
    }
}
