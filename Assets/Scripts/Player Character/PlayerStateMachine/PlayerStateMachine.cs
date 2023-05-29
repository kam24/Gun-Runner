using CharacterSM;
using PlayerMovement.Interfaces;
using PlayerMovement.States;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using static PlayerCharacter;

namespace PlayerMovement
{
    public class PlayerStateMachine: StateMachine
    {
        public PlayerState CurrentState => _extraStatesStack.Count > 0 ? _extraStatesStack.Peek() : BaseState;
        public PlayerState BaseState { get; private set; }

        private List<PlayerState> _baseStates;
        private List<PlayerState> _extraStates;
        private Stack<PlayerState> _extraStatesStack;
        private PlayerCharacter _playerCharacter;
        private bool _isFinished = false;

        public PlayerStateMachine(PlayerCharacter playerCharacter)
        {
            _playerCharacter = playerCharacter;
        }

        public override void Start<T>()
        {
            _baseStates = new List<PlayerState>() 
            { 
                new InAirAfterWallRunState(_playerCharacter, this),
                new InAirState(_playerCharacter, this),
                new RollingState(_playerCharacter, this),
                new RunningState(_playerCharacter, this),
                new ShootingState(_playerCharacter, this),
                new WallRunState(_playerCharacter, this),
            };

            _extraStatesStack = new Stack<PlayerState>(2);
            _extraStates = new List<PlayerState>()
            {
                new StrafeOnLineState(_playerCharacter, this),
                new StrafeOnWallState(_playerCharacter, this),
                new ShootingState(_playerCharacter, this),
            };

            SwitchState<T>();
            Debug.Log(CurrentState);
        }

        public override void SwitchState<T>()
        {
            if (typeof(T) is IExtraState)
                throw new InvalidOperationException();

            BaseState?.Exit();
            BaseState = _baseStates.FirstOrDefault(s => s is T);
            Debug.Log(BaseState + " - After Change Base State");
            BaseState.Enter();
        }

        public void PushExtraState<T>() where T : IExtraState
        {
            var newState = _extraStates.FirstOrDefault(s => s is T);
            _extraStatesStack.Push(newState);
            Debug.Log(CurrentState + " - After Push");
            newState.Enter();
        }

        public void PopExtraState(IExtraState extraState)
        {
            var state = extraState as PlayerState;
            if (_extraStatesStack.Contains(state))
            {
                state.Exit();
                _extraStatesStack.Pop();
            }
            else
                throw new InvalidOperationException();

            if (CurrentState is VerticalState verticalState)
                verticalState.OnExtraStatePoped((IExtraState)state);

            Debug.Log(CurrentState + " - After Pop");
        }

        public override void FixedUpdate(float time)
        {
            if (_isFinished)
                return;

            CurrentState.FixedUpdate(time);
        }

        public void HandleUpKey()
        {
            if (CurrentState.InputHandler.HasUpKeyHandler)
            {
                _playerCharacter.LastStrafe = Strafe.None;
                CurrentState.InputHandler.OnUpKey();
            }
        }

        public void HandleDownKey()
        {
            if (CurrentState.InputHandler.HasDownKeyHandler)
            {
                _playerCharacter.LastStrafe = Strafe.None;
                CurrentState.InputHandler.OnDownKey();
            }
        }

        public void HandleLeftRightKey(Strafe value)
        {
            if (CurrentState.InputHandler.HasLeftRightKeyHandler)
            {
                _playerCharacter.LastStrafe = value;
                CurrentState.InputHandler.OnLeftRightKey();
            }
        }

        public override void Finish()
        {
            CurrentState.Exit();
            _isFinished = true;
        }

        public void OnRollingEnd()
        {
            if (CurrentState is RollingState rollingState)
                rollingState.OnEnding();
        }

    }
}
