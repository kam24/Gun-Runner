using System;

namespace Assets.Scripts
{
    public class SubmachineGun
    {
        public event Action BulletsChanged;
        public event Action AmmoDepleted;

        public ushort Bullets { get; private set; }
        public bool HasBullets => Bullets > 0;

        public SubmachineGun(ushort bullets = 0)
        {
            Bullets = bullets;
        }

        public void AddBullets(ushort count)
        {
            Bullets += count;
            BulletsChanged?.Invoke();
        }

        public void Shoot()
        {
            Bullets -= 1;
            BulletsChanged?.Invoke();
            if (Bullets == 0) 
                AmmoDepleted?.Invoke();
        }
    }
}
