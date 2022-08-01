using System;

namespace Gameplay.Weapons.Shooting
{
    public class BulletEvents
    {
        public event Action<Bullet> Hitted;


        public void OnHit(Bullet bullet)
        {
            Hitted?.Invoke(bullet);
        }
    }
}