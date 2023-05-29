using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PlayerCharacter;

namespace PlayerMovement.States
{
    public class InAirAfterWallRunState : InAirState
    {
        public InAirAfterWallRunState(PlayerCharacter character, PlayerStateMachine stateMachine) : base(character, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            character.SetLongRayDistance();
        }

        public override void Exit()
        {
            base.Exit();
            character.SetNormalRayDistance();
        }

        protected override void DoStrafeOnWall()
        {
            base.DoStrafeOnWall();
            character.JumpForStrafe(character.LastStrafe);
            if (character.LastStrafe == Strafe.Left)
                character.RunLine.GoLeft();
            else
                character.RunLine.GoRight();
        }
    }
}
