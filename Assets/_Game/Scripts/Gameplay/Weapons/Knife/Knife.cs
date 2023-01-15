using System;
using Gameplay.Damaging;
using UnityEngine;
using Zenject;

namespace Gameplay.Weapons.Knife
{
    public class Knife : Weapon
    {
        private Collider[] _colliders;
        private HitSettings _hitSettings;


        [Inject]
        public void Construct(HitSettings hitSettings)
        {
            _hitSettings = hitSettings;

            _colliders = new Collider[_hitSettings.HitCheckCount];
        }

        public override void Initialize()
        {
            AttackType = AttackType.Melee;
        }

        public override void Attack(Vector3 targetPosition)
        {
            var hitCollidersCount = Physics.OverlapSphereNonAlloc(
                transform.position,
                _hitSettings.HitCheckRadius,
                _colliders,
                _hitSettings.HitLayerMask);

            for (var i = 0; i < hitCollidersCount; i++)
                if (_colliders[i].TryGetComponent(out IDamageable damageable)
                    && damageable.DamageGroup != DamageGroup)
                {
                    damageable.ReceiveDamage(
                        Damage,
                        _colliders[i].ClosestPoint(transform.position));
                }
        }

        public class Factory : PlaceholderFactory<Knife>
        {
        }

        [Serializable]
        public class HitSettings
        {
            public LayerMask HitLayerMask;
            [Min(.05f)] public float HitCheckRadius;
            [Min(1)] public int HitCheckCount;
        }
    }
}