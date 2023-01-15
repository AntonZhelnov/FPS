using System;
using Common;
using Common.Animating;
using Common.States;
using UnityEngine;
using Zenject;

namespace Gameplay.Enemy.States
{
    public class EnemyHurtState : IState
    {
        private readonly AnimationPlayer _animationPlayer;
        private readonly Enemy _enemy;
        private readonly Health _health;
        private readonly Settings _settings;
        private readonly LazyInject<EnemyStateMachine> _stateMachine;


        public EnemyHurtState(
            Settings settings,
            Enemy enemy,
            Health health,
            AnimationPlayer animationPlayer,
            LazyInject<EnemyStateMachine> stateMachine)
        {
            _settings = settings;
            _enemy = enemy;
            _health = health;
            _animationPlayer = animationPlayer;
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            if (_health.Property.Value > 0)
            {
                _animationPlayer.Play(
                    _settings.AnimationTriggerName,
                    0f,
                    Continue);
            }
            else
            {
                _animationPlayer.Play(
                    _settings.AnimationTriggerName,
                    _settings.ActionDelay,
                    Die);
            }
        }

        public void Exit()
        {
        }

        public void Tick(float deltaTime)
        {
        }

        private void Continue()
        {
            if (_enemy.IsWithinAttackTargetDistance())
                _stateMachine.Value.Enter<EnemyStunnedState>();
            else
                _stateMachine.Value.Enter<EnemyChasingState>();
        }

        private void Die()
        {
            _stateMachine.Value.Enter<EnemyDeadState>();
        }

        [Serializable]
        public class Settings
        {
            public string AnimationTriggerName;
            [Min(0f)] public float ActionDelay;
        }
    }
}