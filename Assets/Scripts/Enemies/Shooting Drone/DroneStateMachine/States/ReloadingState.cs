using UnityEngine;

namespace DroneStateMachine.States
{
    public class ReloadingState : State
    {
        private Coroutine _coroutine;

        public ReloadingState(ShootingDrone character, IStateSwitcher stateSwitcher) : base(character, stateSwitcher)
        {
        }

        public override void Enter()
        {
            _coroutine = character.StartCoroutine(character.StartTimer(character.ReloadingTime, OnReloadingEnded));
        }

        public override void Exit()
        {
            character.StopCoroutine(_coroutine);
        }

        private void OnReloadingEnded()
        {
            stateSwitcher.SwitchState<AttackState>();
        }
    }
}
