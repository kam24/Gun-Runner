using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class CollectablesController
    {
        private SubmachineGun _gun;
        private Health _playerHealth;
        private PlayerCharacter _player;

        public CollectablesController(SubmachineGun gun, Health playerHealth, PlayerCharacter player) 
        { 
            _gun = gun;
            _playerHealth = playerHealth;
            _player = player;
        }

        public void Register(CollectableBullet bullet)
        {
            bullet.Collected += OnBulletCollected;
        }

        public void Register(CollectableHealth health)
        {
            health.Collected += OnHealthCollected;
        }

        public void Unregister(CollectableBullet bullet)
        {
            bullet.Collected -= OnBulletCollected;
        }
        
        public void Unregister(CollectableHealth health)
        {
            health.Collected -= OnHealthCollected;
        }

        private void OnHealthCollected()
        {
            _playerHealth.RestoreToMax();
        }

        private void OnBulletCollected(ushort _count)
        {
            _gun.AddBullets(_count);
            _player.OnBulletCollected();
        }
    }
}
