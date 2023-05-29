using Assets.Scripts;
using PlayerMovement.Interfaces;
using System;

namespace PlayerMovement.States
{
    public abstract class ShootAbleState : VerticalState, IExtraState
    {
        private SubmachineGun _gun => Root.SubmachineGun;

        public ShootAbleState(PlayerCharacter character, PlayerStateMachine stateMachine) : base(character, stateMachine) { }

        public override void Enter()
        {
            TryStartShooting();
            _gun.AmmoDepleted += OnAmmoDepleted;
            //character.TargetAppeared += TryStartShooting;
        }

        public override void Exit()
        {
            Reset();
            _gun.AmmoDepleted -= OnAmmoDepleted;
            _gun.BulletsChanged -= OnBulletRecieved;
            character.TargetAppeared -= TryStartShooting;
        }

        public override void OnExtraStatePoped(IExtraState extraState)
        {
            if (extraState is not ShootingState)
                TryStartShooting();
        }

        protected override void SwitchState<T>()
        {
            Reset();
            base.SwitchState<T>();
        }

        protected override void PushExtraState<T>()
        {
            Reset();
            base.PushExtraState<T>();
        }
          
        private void TryStartShooting()
        {
            if (_gun.HasBullets)
                stateMachine.PushExtraState<ShootingState>();

            character.TargetAppeared -= TryStartShooting;
        }

        private void Reset() 
        {            
            if (stateMachine.CurrentState is ShootingState shootingState)
                stateMachine.PopExtraState(shootingState); 
        }

        private void OnAmmoDepleted()
        {
            Reset();
            _gun.BulletsChanged += OnBulletRecieved;
        }

        private void OnBulletRecieved()
        {
            if (_gun.Bullets < 1)
                throw new InvalidOperationException();

            TryStartShooting();
            _gun.BulletsChanged -= OnBulletRecieved;
        }
    }
}
