using System;
using Common.Animating;
using Common.States;
using Gameplay.Weapons;
using UnityEngine;
using Zenject;

namespace Gameplay.Enemy.States
{
    public class EnemyAttackingState : IState
    {
        private readonly AnimationPlayer _animationPlayer;
        private readonly Enemy _enemy;
        private readonly Settings _settings;
        private readonly LazyInject<EnemyStateMachine> _stateMachine;
        private readonly EnemyTarget _target;
        private readonly Transform _transform;
        private float _actionDelay;
        private string _animationTriggerName;
        private float _attackDelay;
        private bool _isActual;
        private float _timePassed;


        public EnemyAttackingState(
            Settings settings,
            Enemy enemy,
            AnimationPlayer animationPlayer,
            LazyInject<EnemyStateMachine> stateMachine,
            EnemyTarget target)
        {
            _settings = settings;
            _enemy = enemy;
            _animationPlayer = animationPlayer;
            _stateMachine = stateMachine;
            _target = target;

            _transform = enemy.transform;
        }

        public void Enter()
        {
            _attackDelay = 1f / _settings.AttacksPerSecond;
            _timePassed = _attackDelay;

            _animationTriggerName = _enemy.AttackType switch
            {
                AttackType.Melee => _settings.MeleeAnimationTriggerName,
                AttackType.Ranged => _settings.RangedAnimationTriggerName,
                _ => _animationTriggerName
            };

            _actionDelay = _enemy.AttackType switch
            {
                AttackType.Melee => _settings.MeleeActionDelay,
                AttackType.Ranged => _settings.RangedActionDelay,
                _ => _actionDelay
            };

            _isActual = true;
        }

        public void Exit()
        {
            _isActual = false;
        }

        public void Tick(float deltaTime)
        {
            RotateToTarget(deltaTime);

            if (_enemy.IsWithinAttackTargetDistance())
                WaitForAttack(deltaTime);
            else
                _stateMachine.Value.Enter<EnemyChasingState>();
        }

        private void CompleteAttack()
        {
            if (_isActual)
                _enemy.AttackTarget();
        }

        private void RotateToTarget(float deltaTime)
        {
            var rotation = _transform.rotation;
            rotation =
                Quaternion.RotateTowards(
                    rotation,
                    Quaternion.LookRotation(_target.Transform.position - _transform.position),
                    deltaTime * 100f);
            rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
            _transform.rotation = rotation;
        }

        private void StartAttack()
        {
            _timePassed = 0f;

            var attackDelay =
                _actionDelay > _attackDelay
                    ? _attackDelay
                    : _actionDelay;

            _animationPlayer.Play(
                _animationTriggerName,
                attackDelay,
                CompleteAttack);
        }

        private void WaitForAttack(float deltaTime)
        {
            _timePassed += deltaTime;

            if (_timePassed >= _attackDelay)
                StartAttack();
        }

        [Serializable]
        public class Settings
        {
            [Min(.1f)] public float AttacksPerSecond = 1f;
            public string MeleeAnimationTriggerName;
            [Min(0f)] public float MeleeActionDelay;
            public string RangedAnimationTriggerName;
            [Min(0f)] public float RangedActionDelay;
        }
    }
}