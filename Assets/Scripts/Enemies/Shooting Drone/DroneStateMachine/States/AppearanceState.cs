using CharacterSM;
using UnityEngine;

namespace DroneStateMachine.States
{
    public class AppearanceState : DroneState
    {
        //private Vector3 Position { get => character.transform.position; set => character.transform.position = value; }
        private float _speed;

        public AppearanceState(ShootingDrone character, IStateSwitcher stateSwitcher) : base(character, stateSwitcher)
        {
            _speed = character.AppearanceSpeed;
        }

        public override void FixedUpdate(float time)
        {
            base.FixedUpdate(time);
            var endPosition = new Vector3(character.transform.position.x, character.HeightAboveTarget, character.transform.position.z);
            character.transform.position = Vector3.MoveTowards(character.transform.position, endPosition, _speed * time);
            if (character.transform.position == endPosition)
                stateSwitcher.SwitchState<AttackState>();
        }
    }
}
