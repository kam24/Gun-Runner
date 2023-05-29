using Assets.Scripts;
using DroneStateMachine.States;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace DroneStateMachine
{
    public class StateMachine : IStateSwitcher
    {
        public event Action SwitchingToAttackState;
        public event Action SwitchingToDyingState;

        private Health _health;
        private List<State> _allStates;
        private State _currentState;

        public void Initialize<T>(ShootingDrone character) where T : State
        {
            _allStates = new List<State>()
            {
                new AppearanceState(character,this),
                new AttackState(character,this),
                new ReloadingState(character,this),
                new DyingState(character,this)
            };
            SwitchState<T>();
            _health = character.Health;
            _health.Dying += OnDying;
            //Debug.Log(_currentState);
        }

        private void OnDying()
        {
            SwitchState<DyingState>();
            _health.Dying -= OnDying;
        }

        public void SwitchState<T>() where T : State
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

        public void FixedUpdate(float time)
        {
            _currentState.FixedUpdate(time);
        }

        public void Finish()
        {
            _currentState.Exit();
        }
    }
}