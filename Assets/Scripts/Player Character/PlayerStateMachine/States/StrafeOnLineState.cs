using Assets.Scripts;
using PlayerMovement.Interfaces;
using System;
using static PlayerCharacter;

namespace PlayerMovement.States
{
    public class StrafeOnLineState : StrafeState, IExtraState
    {
        private RunLine _runLine;

        public StrafeOnLineState(PlayerCharacter character, StateMachine stateMachine) : base(character, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            _runLine = character.RunLine;

            if (_strafe == Strafe.None)
            {
                _targetX = _runLine.GetCurrent();
            }
            else
            {
                float? strafeOffset = _strafe == Strafe.Left ? _runLine.GetLeft() : _runLine.GetRight();
                if (strafeOffset.HasValue)
                {
                    _targetX = strafeOffset.Value;
                }
                else
                {
                    stateMachine.PopExtraState(this);
                    return;
                }
            }
        }

        protected override void OnNewPositionAchieved()
        {
            if (_strafe == Strafe.Left)
                _runLine.GoLeft();
            else if (_strafe == Strafe.Right)
                _runLine.GoRight();
        }
    }
}
