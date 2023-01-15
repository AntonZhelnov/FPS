using System;
using Gameplay.Damaging;
using UnityEngine;
using Zenject;

namespace Gameplay.Weapons
{
    public enum AttackType
    {
        Melee,
        Ranged
    }

    public abstract class Weapon : MonoBehaviour, IPoolable<IMemoryPool>
    {
        private IMemoryPool _pool;
        private Settings _settings;

        protected internal AttackType AttackType;
        protected DamageGroup DamageGroup;


        [Inject]
        public void Construct(Settings settings)
        {
            _settings = settings;
        }

        [Inject]
        public virtual void Initialize()
        {
        }

        public float AttackDistance => _settings.AttackDistance;
        protected float AttacksPerSecond => _settings.AttacksPerSecond;
        protected int Damage => _settings.Damage;


        public void OnDespawned()
        {
            _pool = null;
        }

        public void OnSpawned(IMemoryPool pool)
        {
            _pool = pool;
        }

        public abstract void Attack(Vector3 targetPosition);

        public void Equip(
            DamageGroup damageGroup,
            Transform parentTransform)
        {
            DamageGroup = damageGroup;
            SetParent(parentTransform);
        }

        public void Recycle()
        {
            _pool.Despawn(this);
        }

        private void SetParent(Transform parentTransform)
        {
            transform.SetParent(parentTransform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        [Serializable]
        public class Settings
        {
            [Min(.1f)] public float AttackDistance;
            [Min(1)] public int Damage;
            [Min(.1f)] public float AttacksPerSecond;
        }
    }
}