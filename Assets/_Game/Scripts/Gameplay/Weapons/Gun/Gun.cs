using UnityEngine;
using Zenject;

namespace Gameplay.Weapons.Gun
{
    public class Gun : Weapon
    {
        [SerializeField] private Transform _shootingTransform;

        private Shooter _shooter;
        private Shooter.Factory _shooterFactory;


        [Inject]
        public void Construct(Shooter.Factory shooterFactory)
        {
            _shooterFactory = shooterFactory;
        }

        public override void Initialize()
        {
            AttackType = AttackType.Ranged;
        }

        public void Start()
        {
            _shooter = _shooterFactory.Create(
                _shootingTransform,
                AttacksPerSecond);
        }

        public override void Attack(Vector3 targetPosition)
        {
            _shooter.Shoot(
                DamageGroup,
                targetPosition);
        }

        public class Factory : PlaceholderFactory<Gun>
        {
        }
    }
}