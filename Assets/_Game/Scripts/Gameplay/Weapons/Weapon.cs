using System;
using Common;
using Configs.Weapons;
using UnityEngine;
using Zenject;

namespace Gameplay.Weapons
{
    [Serializable]
    public abstract class Weapon : MonoBehaviour, IInitializable
    {
        private WeaponConfig _weaponConfig;

        protected int Damage;
        protected DamageGroup DamageGroup;


        [Inject]
        public void Construct(
            WeaponConfig weaponConfig,
            DamageGroup damageGroup)
        {
            _weaponConfig = weaponConfig;
            DamageGroup = damageGroup;
        }

        [Inject]
        public void Initialize()
        {
            AttackDistance = _weaponConfig.AttackDistance;
            Damage = _weaponConfig.Damage;
            AttacksPerSecond = _weaponConfig.AttacksPerSecond;
        }

        public float AttackDistance { get; private set; }
        public float AttacksPerSecond { get; private set; }


        public abstract void Attack(Vector3 targetPosition);

        public void SetParent(Transform parentTransform)
        {
            transform.SetParent(parentTransform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
    }
}