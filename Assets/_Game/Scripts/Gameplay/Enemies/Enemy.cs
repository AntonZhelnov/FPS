using System;
using System.Collections.Generic;
using Common;
using Common.Pausing;
using Common.States;
using Configs.Enemies;
using Gameplay.Enemies.States;
using Gameplay.Player;
using Gameplay.Weapons;
using Gameplay.Weapons.Factories;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay.Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Collider))]
    public class Enemy : MonoBehaviour, IDamageable, IPausable, IPoolable<IMemoryPool>, IInitializable
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _weaponHolderTransform;

        private float _animatorSpeed;
        private float _attackDelay;
        private float _attackDistanceSquared;
        private AttackingPlayerEnemyState.Factory _attackingPlayerEnemyStateFactory;
        private ChasingPlayerEnemyState.Factory _chasingPlayerEnemyStateFactory;
        private Collider _collider;
        private DeadEnemyState.Factory _deadEnemyStateFactory;
        private EnemyArchetypeConfig _enemyArchetypeConfig;
        private List<EnemyArchetypeConfig> _enemyArchetypeConfigs;
        private EnemyAttackConfig _enemyAttackConfig;
        private List<EnemyAttackConfig> _enemyAttackConfigs;
        private EnemyConfig _enemyConfig;
        private EnemyEvents _enemyEvents;
        private Health _health;
        private Health.Factory _healthFactory;
        private HurtEnemyState.Factory _hurtEnemyStateFactory;
        private IdleEnemyState.Factory _idleEnemyStateFactory;
        private NavMeshAgent _navMeshAgent;
        private Pauser _pauser;
        private PlayerCharacter _playerCharacter;
        private Transform _playerTransform;
        private IMemoryPool _pool;
        private StateMachine _stateMachine;
        private StateMachine.Factory _stateMachineFactory;
        private StunEnemyState.Factory _stunEnemyStateFactory;
        private Weapon _weapon;
        private WeaponFactory _weaponFactory;


        [Inject]
        public void Construct(
            DamageGroup damageGroup,
            EnemyConfig enemyConfig,
            EnemyEvents enemyEvents,
            Health.Factory healthFactory,
            WeaponFactory weaponFactory,
            StateMachine.Factory stateMachineFactory,
            ChasingPlayerEnemyState.Factory chasingPlayerEnemyStateFactory,
            AttackingPlayerEnemyState.Factory attackingPlayerEnemyStateFactory,
            HurtEnemyState.Factory hurtEnemyStateFactory,
            StunEnemyState.Factory stunEnemyStateFactory,
            IdleEnemyState.Factory idleEnemyStateFactory,
            DeadEnemyState.Factory deadEnemyStateFactory,
            PlayerCharacter playerCharacter,
            Pauser pauser)
        {
            DamageGroup = damageGroup;
            _enemyConfig = enemyConfig;
            _enemyEvents = enemyEvents;
            _healthFactory = healthFactory;
            _weaponFactory = weaponFactory;
            _stateMachineFactory = stateMachineFactory;
            _chasingPlayerEnemyStateFactory = chasingPlayerEnemyStateFactory;
            _attackingPlayerEnemyStateFactory = attackingPlayerEnemyStateFactory;
            _hurtEnemyStateFactory = hurtEnemyStateFactory;
            _stunEnemyStateFactory = stunEnemyStateFactory;
            _idleEnemyStateFactory = idleEnemyStateFactory;
            _deadEnemyStateFactory = deadEnemyStateFactory;
            _playerCharacter = playerCharacter;
            _pauser = pauser;
        }

        [Inject]
        public void Initialize()
        {
            _enemyArchetypeConfigs = _enemyConfig.EnemyArchetypeConfigs;
            _enemyArchetypeConfig = GetRandomEnemyArchetypeConfig();
            _health = _healthFactory.Create(_enemyArchetypeConfig.Health);

            _navMeshAgent = GetComponent<NavMeshAgent>();
            _collider = GetComponent<Collider>();

            _playerTransform = _playerCharacter.transform;
            Score = _enemyArchetypeConfig.Score;

            InitializeAttack();
            InitializeStateMachine();
        }

        public Animator Animator => _animator;
        public int Score { get; private set; }
        public DamageGroup DamageGroup { get; private set; }


        public void ReceiveDamage(int damage)
        {
            if (_health.TryDecrease(damage))
                _stateMachine.SwitchState<HurtEnemyState>();
        }

        public void Pause()
        {
            _navMeshAgent.enabled = false;
            _animatorSpeed = _animator.speed;
            _animator.speed = 0f;
        }

        public void Resume()
        {
            _navMeshAgent.enabled = true;
            _animator.speed = _animatorSpeed;
        }

        public void OnDespawned()
        {
            _stateMachine.Stop();
            _health.Depleted -= Die;
            _pool = null;
            _pauser.Unregister(this);
        }

        public void OnSpawned(IMemoryPool pool)
        {
            _enemyEvents.OnSpawned(this);
            _collider.enabled = true;
            _health.Reset();
            _health.Depleted += Die;
            _pool = pool;
            _pauser.Register(this);
            _stateMachine.Start();
        }

        public void StayStill()
        {
            _stateMachine.SwitchState<IdleEnemyState>();
            _stateMachine.Stop();
        }

        protected internal bool IsWithinAttackRange()
        {
            return (transform.position - _playerTransform.position).sqrMagnitude <= _attackDistanceSquared;
        }

        internal void Recycle()
        {
            _pool.Despawn(this);
        }

        private void Die()
        {
            _stateMachine.SwitchState<DeadEnemyState>();
            _stateMachine.Stop();
        }

        private EnemyArchetypeConfig GetRandomEnemyArchetypeConfig()
        {
            return _enemyArchetypeConfigs[Random.Range(0, _enemyArchetypeConfigs.Count)];
        }

        private EnemyAttackConfig GetRandomEnemyAttackConfig()
        {
            return _enemyAttackConfigs[Random.Range(0, _enemyAttackConfigs.Count)];
        }

        private void InitializeAttack()
        {
            _enemyAttackConfigs = _enemyConfig.EnemyAttackConfigs;
            _enemyAttackConfig = GetRandomEnemyAttackConfig();

            _weapon = _weaponFactory.Create(_enemyAttackConfig.Weapon.GetType(), DamageGroup);
            _weapon.SetParent(_weaponHolderTransform);
            _attackDistanceSquared = (float)Math.Pow(_weapon.AttackDistance, 2);

            _attackDelay = 1f / _enemyArchetypeConfig.AttacksPerSecond;
        }

        private void InitializeStateMachine()
        {
            _stateMachine = _stateMachineFactory.Create(
                new List<State>
                {
                    _chasingPlayerEnemyStateFactory.Create(
                        this,
                        _navMeshAgent,
                        _playerTransform,
                        _enemyConfig.MoveAnimationConfig),

                    _attackingPlayerEnemyStateFactory.Create(
                        this,
                        _playerTransform,
                        _weapon,
                        _attackDelay,
                        _enemyAttackConfig.AttackAnimationConfig),

                    _hurtEnemyStateFactory.Create(
                        this,
                        _enemyConfig.HurtAnimationConfig),

                    _stunEnemyStateFactory.Create(
                        this,
                        _enemyConfig.IdleAnimationConfig),

                    _idleEnemyStateFactory.Create(
                        this,
                        _enemyConfig.IdleAnimationConfig),

                    _deadEnemyStateFactory.Create(
                        this,
                        _enemyEvents,
                        _navMeshAgent,
                        _collider,
                        _enemyConfig.DeadAnimationConfig)
                });
        }

        public class Factory : PlaceholderFactory<Enemy>
        {
        }
    }
}