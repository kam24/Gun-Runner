using PlayerMovement.Interfaces;
using static PlayerCharacter;

namespace PlayerMovement.States
{
    public abstract class VerticalState : BaseState
    {
        protected bool _startJump = false;

        public VerticalState(PlayerCharacter character, StateMachine stateMachine) : base(character, stateMachine) 
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
            ChangeBaseState(character.InAirState);
        }

        protected void DoStrafe()        
        {
            Strafe strafe = character.LastStrafe;
            if (strafe == Strafe.None)
                PushExtraState(character.StrafeOnLineState);
            else if (character.CheckWallAhead(strafe))
                DoStrafeOnWall();
            else
                PushExtraState(character.StrafeOnLineState);
        }

        protected virtual void DoStrafeOnWall()
        {
            PushExtraState(character.StrafeOnWallState);
        }

        protected virtual void ChangeBaseState(BaseState state)
        {
            stateMachine.ChangeBaseState(state);
        }

        protected virtual void PushExtraState(IExtraState state)
        {
            stateMachine.PushExtraState(state);
        }
    }    
}
