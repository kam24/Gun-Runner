using UnityEngine;

namespace DroneStateMachine.States
{
    public class AttackState : State
    {
        private Coroutine _coroutine;
        private uint _projectiles;
        private uint _maxProjectiles;

        public AttackState(ShootingDrone character, IStateSwitcher stateSwitcher) : base(character, stateSwitcher)
        {
            _maxProjectiles = character.MaxProjectiles;
        }

        public override void Enter()
        {
            _projectiles = _maxProjectiles;
            _coroutine = character.StartCoroutine(character.Shoot());
            character.Shot += OnShot;
        }

        public override void Exit()
        {
            character.StopCoroutine(_coroutine);
            character.Shot -= OnShot;
        }

        public override void FixedUpdate(float time)
        {
            base.FixedUpdate(time);

            if (_projectiles == 0)
            {
                stateSwitcher.SwitchState<ReloadingState>();
                return;
            }

            character.TurnTowardsTarget();
        }

        private void OnShot()
        {
            _projectiles--;
        }
    }
}
