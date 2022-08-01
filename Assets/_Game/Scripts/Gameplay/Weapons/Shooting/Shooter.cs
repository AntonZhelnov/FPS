using Common;
using UnityEngine;
using Zenject;

namespace Gameplay.Weapons.Shooting
{
    public class Shooter : ITickable, IInitializable
    {
        private readonly Bullet.Factory _bulletFactory;
        private readonly float _bulletsPerSecond;
        private readonly DamageGroup _damageGroup;
        private readonly Transform _shooterTransform;
        private readonly TickableManager _tickableManager;
        private float _shotDelay;
        private float _timePassed;


        public Shooter(
            Bullet.Factory bulletFactory,
            Transform shooterTransform,
            float bulletsPerSecond,
            TickableManager tickableManager,
            DamageGroup damageGroup)
        {
            _bulletFactory = bulletFactory;
            _shooterTransform = shooterTransform;
            _bulletsPerSecond = bulletsPerSecond;
            _tickableManager = tickableManager;
            _damageGroup = damageGroup;
        }

        [Inject]
        public void Initialize()
        {
            _tickableManager.Add(this);
            _shotDelay = 1f / _bulletsPerSecond;
            _timePassed = _shotDelay;
        }

        public void Tick()
        {
            _timePassed += Time.deltaTime;
        }

        public void Shoot(Vector3 targetPosition)
        {
            if (_timePassed > _shotDelay)
            {
                ShootBullet(targetPosition);
                _timePassed = 0f;
            }
        }

        private void ShootBullet(Vector3 targetPosition)
        {
            var bullet = _bulletFactory.Create(_damageGroup);

            bullet.transform.position = _shooterTransform.position;
            bullet.transform.LookAt(targetPosition);
        }

        public class Factory : PlaceholderFactory<DamageGroup, Transform, float, Shooter>
        {
        }
    }
}