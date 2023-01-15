using Gameplay.Damaging;
using UnityEngine;
using Zenject;

namespace Gameplay.Weapons.Gun
{
    public class Shooter : IInitializable, ITickable
    {
        private readonly Bullet.Factory _bulletFactory;
        private readonly float _bulletsPerSecond;
        private readonly Transform _shooterTransform;
        private readonly TickableManager _tickableManager;
        private float _shotDelay;
        private float _timePassed;


        public Shooter(
            Transform shooterTransform,
            Bullet.Factory bulletFactory,
            float bulletsPerSecond,
            TickableManager tickableManager)
        {
            _shooterTransform = shooterTransform;
            _bulletFactory = bulletFactory;
            _bulletsPerSecond = bulletsPerSecond;
            _tickableManager = tickableManager;
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

        public void Shoot(
            DamageGroup damageGroup,
            Vector3 targetPosition)
        {
            if (_timePassed > _shotDelay)
            {
                var bullet = _bulletFactory.Create(damageGroup);
                bullet.transform.position = _shooterTransform.position;
                bullet.transform.LookAt(targetPosition);

                _timePassed = 0f;
            }
        }

        public class Factory : PlaceholderFactory<Transform, float, Shooter>
        {
        }
    }
}