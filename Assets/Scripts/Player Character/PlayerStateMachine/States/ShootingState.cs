using PlayerMovement.Interfaces;
using System.Collections;
using UnityEngine;

namespace PlayerMovement.States
{
    public class ShootingState : BaseState, IExtraState
    {
        public override InputHandler InputHandler => stateMachine.BaseState.InputHandler;

        private Coroutine _readyToAimTimer;
        private Coroutine _readyToShootTimer;
        private Coroutine _shooting;

        private bool _readyToAim;
        private bool _aiming;

        public ShootingState(PlayerCharacter character, StateMachine stateMachine) : base(character, stateMachine) { }

        public override void Enter()
        {
            _readyToAim = false;
            _aiming = false;
            _readyToAimTimer = character.StartReadyToAimTimer(OnReadyToAim);
        }

        public override void Exit()
        {
            _readyToAim = false;
            _aiming = false;

            TryStopCoroutine(_readyToAimTimer);
            TryStopCoroutine(_readyToShootTimer);
            TryStopCoroutine(_shooting);

            character.SetAnimationBool(character.AnimIDShooting, false);
        }

        public override void FixedUpdate(float time)
        {
            stateMachine.BaseState.FixedUpdate(time);

            if (character.IsTargetAppeared && _readyToAim && _aiming == false)
            {
                _aiming = true;
                character.SetAnimationBool(character.AnimIDShooting, true);
            }

            if (_aiming && character.IsTargetAppeared == false)
            {
                _aiming = false;
                character.SetAnimationBool(character.AnimIDShooting, false);
            }
        }

        private void OnReadyToAim()
        {
            _readyToAim = true;
            _readyToShootTimer = character.StartReadyToShotTimer(OnStartShooting);
        }

        private void OnStartShooting()
        {
            _shooting = character.StartCoroutine(character.StartShooting());
            //character.StartCoroutine(_shoot);
        }

        private void TryStopCoroutine(Coroutine coroutine)
        {
            if (coroutine != null)
            {
                character.StopCoroutine(coroutine);
            }
        }
    }
}
