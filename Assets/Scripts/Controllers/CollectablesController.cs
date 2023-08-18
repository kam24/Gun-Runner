namespace Assets.Scripts
{
    public class CollectablesController
    {
        private SubmachineGun _gun;
        private Health _playerHealth;
        private PlayerCharacter _player;

        public CollectablesController(SubmachineGun gun, PlayerCharacter player)
        {
            _gun = gun;
            _player = player;
        }

        public void Register(CollectableBullet bullet)
        {
            bullet.Collected += OnBulletCollected;
        }

        public void Unregister(CollectableBullet bullet)
        {
            bullet.Collected -= OnBulletCollected;
        }

        private void OnBulletCollected(ushort _count)
        {
            _gun.AddBullets(_count);
            _player.OnBulletCollected();
        }
    }
}
