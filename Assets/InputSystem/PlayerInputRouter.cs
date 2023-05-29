using static PlayerCharacter;
using static UnityEngine.InputSystem.InputAction;

namespace Assets.InputSystem
{
    public class PlayerInputRouter
    {
        private PlayerInput _input;
        private PlayerCharacter _character;

        public PlayerInputRouter(PlayerCharacter character)
        {
            _input = new();
            _character = character;
        }

        public void OnEnable()
        {
            _input.Enable();
            _input.Player.Jump.performed += OnJump;
            _input.Player.FallDawnRoll.performed += OnDown;
            _input.Player.LeftStrafe.performed += OnLeftStrafe;
            _input.Player.RightStrafe.performed += OnRightStrafe;
        }

        public void OnDisable()
        {
            _input.Disable();
            _input.Player.Jump.performed -= OnJump;
            _input.Player.FallDawnRoll.performed -= OnDown;
            _input.Player.LeftStrafe.performed -= OnLeftStrafe;
            _input.Player.RightStrafe.performed -= OnRightStrafe;
        }

        private void OnRightStrafe(CallbackContext obj)
        {
            _character.OnLeftRightKey(Strafe.Right);
        }

        private void OnLeftStrafe(CallbackContext obj)
        {
            _character.OnLeftRightKey(Strafe.Left);
        }

        private void OnJump(CallbackContext obj)
        {
            if (_character.enabled)
                _character.OnUpKey();
        }

        private void OnDown(CallbackContext obj)
        {
            if (_character.enabled)
                _character.OnDownKey();
        }
    }
}
