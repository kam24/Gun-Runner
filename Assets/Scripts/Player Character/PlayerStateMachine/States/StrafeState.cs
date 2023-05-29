using Assets.Scripts;
using PlayerMovement.Interfaces;
using UnityEngine;
using static PlayerCharacter;

namespace PlayerMovement.States
{
    public abstract class StrafeState : PlayerState, IExtraState
    {
        protected Strafe _strafe;
        protected float _targetX;

        public StrafeState(PlayerCharacter character, PlayerStateMachine stateMachine) : base(character, stateMachine) { }

        public override void Enter()
        {
            _strafe = character.LastStrafe;
        }

        public override void FixedUpdate(float time)
        {
            var positionX = character.transform.position.x;
            var newPositionX = Mathf.MoveTowards(positionX, _targetX, character.HorizontalSpeed * time);

            if (newPositionX == positionX)
            {
                stateMachine.PopExtraState(this);
                OnNewPositionAchieved();
            }
            else
            {
                float offsetX = newPositionX - positionX;
                character.Move(offsetX);
            }
        }

        protected abstract void OnNewPositionAchieved();
    }
}
