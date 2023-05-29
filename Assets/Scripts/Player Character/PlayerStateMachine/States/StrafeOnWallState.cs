using PlayerMovement.Interfaces;
using static PlayerCharacter;

namespace PlayerMovement.States
{
    public class StrafeOnWallState : StrafeState, IExtraState
    {
        public StrafeOnWallState(PlayerCharacter character, StateMachine stateMachine) : base(character, stateMachine) { }

        public override void Enter()
        {
            base.Enter();

            if (character.IsGrounded())
                character.JumpForStrafe(_strafe);

            _targetX = character.GetWallRunPositionX(_strafe);
        }

        protected override void OnNewPositionAchieved()
        {
            bool strafeDirection = false;
            if (_strafe == Strafe.Left)
            {
                character.WallRunning = WallRun.Right;
                strafeDirection = false;
            }
            else if (_strafe == Strafe.Right)
            {
                character.WallRunning = WallRun.Left;
                strafeDirection = true;
            }
            character.SetAnimationBool(character.AnimIDStrafeDirection, strafeDirection);
            stateMachine.ChangeBaseState(character.WallRunState);
            return;
        }
    }
}
