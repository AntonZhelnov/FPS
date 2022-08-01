using Common;
using Configs;
using Gameplay.Weapons;
using UnityEngine;
using Zenject;

namespace Gameplay.Enemies.States
{
    public class AttackingPlayerEnemyState : EnemyState
    {
        private readonly float _attackDelay;
        private readonly Transform _playerTransform;
        private readonly Weapon _weapon;
        private Transform _enemyTransform;
        private bool _isActual;
        private float _timePassed;


        public AttackingPlayerEnemyState(
            Enemy enemy,
            Transform playerTransform,
            Weapon weapon,
            float attackDelay,
            AnimationConfig animationConfig,
            AnimationPlayer.Factory animationPlayerFactory)
            : base(enemy, animationConfig, animationPlayerFactory)
        {
            _playerTransform = playerTransform;
            _weapon = weapon;
            _attackDelay = attackDelay;
        }

        public override void Initialize()
        {
            base.Initialize();
            _enemyTransform = Enemy.transform;
        }

        public override void Start()
        {
            _isActual = true;
            _timePassed = _attackDelay;
        }

        public override void Stop()
        {
            _isActual = false;
        }

        public override void UpdateTime(float deltaTime)
        {
            RotateToPlayer(deltaTime);

            if (!Enemy.IsWithinAttackRange())
                StateMachine.SwitchState<ChasingPlayerEnemyState>();
            else
                StartAttack(deltaTime);
        }

        protected override void AnimationPlayerCallback()
        {
            CompleteAttack();
        }

        private void CompleteAttack()
        {
            if (_isActual
                && _playerTransform)
            {
                var playerPosition = _playerTransform.position;
                _weapon.Attack(new Vector3(playerPosition.x, 1f, playerPosition.z));
            }
        }

        private void RotateToPlayer(float deltaTime)
        {
            var rotation = _enemyTransform.rotation;
            rotation =
                Quaternion.RotateTowards(
                    rotation,
                    Quaternion.LookRotation(_playerTransform.position - _enemyTransform.position),
                    deltaTime * 100f);
            rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
            _enemyTransform.rotation = rotation;
        }

        private void StartAttack(float deltaTime)
        {
            _timePassed += deltaTime;
            if (_timePassed >= _attackDelay)
            {
                _timePassed = 0f;
                AnimationPlayer.Play();
            }
        }

        public class Factory : PlaceholderFactory<Enemy, Transform, Weapon, float, AnimationConfig,
            AttackingPlayerEnemyState>
        {
        }
    }
}