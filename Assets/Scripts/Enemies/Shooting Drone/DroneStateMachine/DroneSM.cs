using Assets.Scripts;
using CharacterSM;
using DroneStateMachine.States;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DroneStateMachine
{
    public class DroneSM : StateMachine, IDisposable
    {
        public event Action SwitchingToAttackState;
        public event Action SwitchingToDyingState;

        private ShootingDrone _character;
        private Health _health;
        private List<DroneState> _allStates;
        private DroneState _currentState;

        public DroneSM(ShootingDrone character)
        {
            _character = character;
        }

        public override void Start<T>()
        {
            _allStates = new List<DroneState>()
            {
                new AppearanceState(_character,this),
                new AttackState(_character,this),
                new ReloadingState(_character,this),
                new DyingState(_character,this)
            };
            SwitchState<T>();
            _health = _character.Health;
            _health.Dying += OnDying;
            //Debug.Log(_currentState);
        }

        private void OnDying()
        {
            SwitchState<DyingState>();
            _health.Dying -= OnDying;
        }

        public override void SwitchState<T>()
        {
            _currentState?.Exit();
            _currentState = _allStates.FirstOrDefault(s => s is T);
            _currentState.Enter();

            if (_currentState is AttackState)
                SwitchingToAttackState?.Invoke();
            if (_currentState is DyingState)
                SwitchingToDyingState?.Invoke();

            //Debug.Log(_currentState);
        }

        public override void FixedUpdate(float time)
        {
            _currentState.FixedUpdate(time);
        }

        public override void Finish()
        {
            _currentState.Exit();
        }

        public void Dispose()
        {
            _health.Dying -= OnDying;
        }
    }
}