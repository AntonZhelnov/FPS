using System;
using Common;
using Gameplay.Damaging;
using Gameplay.Enemy.States;
using Gameplay.Levels;
using Gameplay.Player;
using Gameplay.Weapons;
using UnityEngine;
using Zenject;

namespace Gameplay.Enemy
{
    public class Enemy : MonoBehaviour, IDamageable, IWeaponUser, IPoolable<IMemoryPool>
    {
        private float _attackDistanceSquared;
        private DamageReactor _damageReactor;
        private Health _health;
        private IMemoryPool _pool;
        private Settings _settings;
        private SignalBus _signalBus;
        private EnemyStateMachine _stateMachine;
        private EnemyTarget _target;
        private WeaponHolder _weaponHolder;


        [Inject]
        public void Construct(
            Settings settings,
            Health health,
            DamageGroup damageGroup,
            DamageReactor damageReactor,
            WeaponHolder weaponHolder,
            EnemyStateMachine stateMachine,
            EnemyTarget target,
            SignalBus signalBus)
        {
            _settings = settings;
            _health = health;
            DamageGroup = damageGroup;
            _damageReactor = damageReactor;
            _weaponHolder = weaponHolder;
            _stateMachine = stateMachine;
            _target = target;
            _signalBus = signalBus;
        }

        public int Score => _settings.Score;
        public AttackType AttackType { get; private set; }
        public DamageGroup DamageGroup { get; private set; }


        public void ReceiveDamage(
            int damage,
            Vector3 position)
        {
            _damageReactor.React(
                damage,
                position);
        }

        public void OnDespawned()
        {
            _stateMachine.Exit();
            _health.Decreased -= OnHealthDecreased;
            _signalBus.Unsubscribe<PlayerDiedSignal>(Stay);
            _signalBus.Unsubscribe<LevelRestartingSignal>(Recycle);
            _pool = null;
        }

        public void OnSpawned(IMemoryPool pool)
        {
            _health.Decreased += OnHealthDecreased;
            _signalBus.Subscribe<PlayerDiedSignal>(Stay);
            _signalBus.Subscribe<LevelRestartingSignal>(Recycle);
            _health.Reset();
            _stateMachine.Enter<EnemyChasingState>();
            _pool = pool;
        }

        public void EquipWeapon(Weapon weapon)
        {
            _weaponHolder.Equip(weapon);
            AttackType = weapon.AttackType;
            _attackDistanceSquared = (float)Math.Pow(weapon.AttackDistance, 2);
        }

        protected internal void AttackTarget()
        {
            var targetPosition = _target.Transform.position;
            _weaponHolder.Use(new Vector3(targetPosition.x, 1f, targetPosition.z));
        }

        protected internal bool IsWithinAttackTargetDistance()
        {
            return (transform.position - _target.Transform.position).sqrMagnitude <= _attackDistanceSquared;
        }

        internal void Recycle()
        {
            _pool.Despawn(this);
        }

        private void OnHealthDecreased()
        {
            _stateMachine.Enter<EnemyHurtState>();
        }

        private void Stay()
        {
            _stateMachine.Enter<EnemyIdleState>();
            _stateMachine.Exit();
        }

        public class Factory : PlaceholderFactory<Enemy>
        {
        }

        [Serializable]
        public class Settings
        {
            [Min(1)] public int Health = 1;
            [Min(1)] public int Score = 1;
        }
    }
}