using Common;
using Gameplay.Weapons.Shooting;
using UnityEngine;
using Zenject;

namespace Gameplay.Weapons
{
    public class Gun : Weapon, IInitializable
    {
        [SerializeField] private Transform _shootingTransform;

        private Shooter _shooter;
        private Shooter.Factory _shooterFactory;


        [Inject]
        public void Construct(Shooter.Factory shooterFactory)
        {
            _shooterFactory = shooterFactory;
        }

        [Inject]
        public new void Initialize()
        {
            base.Initialize();

            var shooterTransform = _shootingTransform;
            var bulletsPerSecond = AttacksPerSecond;

            _shooter = _shooterFactory.Create(
                DamageGroup,
                shooterTransform,
                bulletsPerSecond);
        }

        public override void Attack(Vector3 targetPosition)
        {
            _shooter.Shoot(targetPosition);
        }

        public class Factory : PlaceholderFactory<DamageGroup, Gun>
        {
        }
    }
}