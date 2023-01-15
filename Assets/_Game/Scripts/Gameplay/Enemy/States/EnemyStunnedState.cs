using System;
using Common.Animating;
using Common.States;
using UnityEngine;
using Zenject;

namespace Gameplay.Enemy.States
{
    public class EnemyStunnedState : IState
    {
        private readonly AnimationPlayer _animationPlayer;
        private readonly Enemy _enemy;
        private readonly Settings _settings;
        private readonly LazyInject<EnemyStateMachine> _stateMachine;


        public EnemyStunnedState(
            Settings settings,
            Enemy enemy,
            AnimationPlayer animationPlayer,
            LazyInject<EnemyStateMachine> stateMachine)
        {
            _settings = settings;
            _enemy = enemy;
            _animationPlayer = animationPlayer;
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _animationPlayer.Play(
                _settings.AnimationTriggerName,
                _settings.ActionDelay,
                Continue);
        }

        public void Exit()
        {
            _animationPlayer.Stop();
        }

        public void Tick(float deltaTime)
        {
        }

        private void Continue()
        {
            if (_enemy.IsWithinAttackTargetDistance())
                _stateMachine.Value.Enter<EnemyAttackingState>();
            else
                _stateMachine.Value.Enter<EnemyChasingState>();
        }

        [Serializable]
        public class Settings
        {
            public string AnimationTriggerName;
            [Min(0f)] public float ActionDelay;
        }
    }
}