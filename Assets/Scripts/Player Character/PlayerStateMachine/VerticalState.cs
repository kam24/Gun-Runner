using PlayerMovement.Interfaces;
using static PlayerCharacter;

namespace PlayerMovement.States
{
    public abstract class VerticalState : PlayerState
    {
        protected bool _startJump = false;

        public VerticalState(PlayerCharacter character, PlayerStateMachine stateMachine) : base(character, stateMachine) 
        {
            InputHandler.SetLeftRightKeyHandler(OnLeftRightKey);
        }

        public virtual void OnExtraStatePoped(IExtraState extraState)
        {

        }

        public virtual void OnLeftRightKey()
        {
            DoStrafe();
        }

        protected void Jump()
        {
            character.Jump();
            _startJump = false;
            SwitchState<InAirState>();
        }

        protected void DoStrafe()        
        {
            Strafe strafe = character.LastStrafe;
            if (strafe == Strafe.None)
                PushExtraState<StrafeOnLineState>();
            else if (character.CheckWallAhead(strafe))
                DoStrafeOnWall();
            else
                PushExtraState<StrafeOnLineState>();
        }

        protected virtual void DoStrafeOnWall()
        {
            PushExtraState<StrafeOnWallState>();
        }

        protected virtual void SwitchState<T>() where T: PlayerState
        {
            stateMachine.SwitchState<T>();
        }

        protected virtual void PushExtraState<T>() where T : IExtraState
        {
            stateMachine.PushExtraState<T>();
        }
    }    
}
