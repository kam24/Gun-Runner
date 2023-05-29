using PlayerMovement.Interfaces;
using PlayerMovement.States;
using System;
using System.Collections.Generic;
using static PlayerCharacter;

namespace PlayerMovement
{
    public class StateMachine
    {
        public BaseState CurrentState => _extraStates.Count > 0 ? _extraStates.Peek() : BaseState;
        public BaseState BaseState { get; private set; }

        private Stack<BaseState> _extraStates;
        private PlayerCharacter _playerCharacter;
        private bool _isFinished = false;

        public void Initialize(VerticalState startingState, PlayerCharacter playerCharacter)
        {
            _playerCharacter = playerCharacter;
            _extraStates = new Stack<BaseState>(2);
            BaseState = startingState;
            BaseState.Enter();
            //Debug.Log(CurrentState);
        }

        public void ChangeBaseState(BaseState newState)
        {
            BaseState.Exit();
            BaseState = newState;
            //Debug.Log(BaseState + " - After Change Base State");
            BaseState.Enter();
        }

        public void PushExtraState(IExtraState state)
        {
            var newState = state as BaseState;
            _extraStates.Push(newState);
            //Debug.Log(CurrentState + " - After Push");
            newState.Enter();
        }

        public void PopExtraState(IExtraState extraState)
        {
            var state = extraState as BaseState;
            if (_extraStates.Contains(state))
            {
                state.Exit();
                _extraStates.Pop();
            }
            else
                throw new InvalidOperationException();

            if (CurrentState is VerticalState verticalState)
                verticalState.OnExtraStatePoped(extraState);

            //Debug.Log(CurrentState + " - After Pop");
        }

        public void FixedUpdate(float time)
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

        public void Finish()
        {
            CurrentState.Exit();
            _isFinished = true;
        }
    }
}
