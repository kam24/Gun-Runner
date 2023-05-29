using CharacterSM;
using UnityEngine;

namespace DroneStateMachine.States
{
    public class DyingState : DroneState
    {
        private bool _fallingDown;
        public DyingState(ShootingDrone character, IStateSwitcher stateSwitcher) : base(character, stateSwitcher)
        {
            _fallingDown = false;
        }

        public override void Enter()
        {
            character.transform.rotation = Quaternion.identity;
            character.DisableHealthBar();
            character.StartCoroutine(character.StartTimer(character.DelayBeforeFallingDown, () => _fallingDown = true));
        }

        public override void FixedUpdate(float time)
        {
            character.RotateAroundItself();
            if (_fallingDown)
                character.FallDownOnTargetLine();
            else
                character.MoveForward();
        }
    }
}
