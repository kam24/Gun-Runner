using Assets.Scripts;
using PlayerMovement.Interfaces;
using System;

namespace PlayerMovement.States
{
    public abstract class ShootAbleState : VerticalState
    {
        private SubmachineGun _gun => Root.SubmachineGun;

        public ShootAbleState(PlayerCharacter character, StateMachine stateMachine) : base(character, stateMachine) { }

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

        protected override void ChangeBaseState(BaseState state)
        {
            Reset();
            base.ChangeBaseState(state);
        }

        protected override void PushExtraState(IExtraState state)
        {
            Reset();
            base.PushExtraState(state);
        }
          
        private void TryStartShooting()
        {
            if (_gun.HasBullets)
                stateMachine.PushExtraState(character.ShootingState);

            character.TargetAppeared -= TryStartShooting;
        }

        private void Reset() 
        {            
            if (stateMachine.CurrentState is ShootingState)
                stateMachine.PopExtraState(character.ShootingState); 
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
