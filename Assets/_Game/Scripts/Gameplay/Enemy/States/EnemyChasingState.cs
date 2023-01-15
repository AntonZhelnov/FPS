using System;
using Common.Animating;
using Common.States;
using UnityEngine.AI;
using Zenject;

namespace Gameplay.Enemy.States
{
    public class EnemyChasingState : IState
    {
        private readonly AnimationPlayer _animationPlayer;
        private readonly Enemy _enemy;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly Settings _settings;
        private readonly LazyInject<EnemyStateMachine> _stateMachine;
        private readonly EnemyTarget _target;
        private int _framesSkipped;


        public EnemyChasingState(
            Settings settings,
            Enemy enemy,
            NavMeshAgent navMeshAgent,
            AnimationPlayer animationPlayer,
            LazyInject<EnemyStateMachine> stateMachine,
            EnemyTarget target)
        {
            _settings = settings;
            _enemy = enemy;
            _navMeshAgent = navMeshAgent;
            _animationPlayer = animationPlayer;
            _stateMachine = stateMachine;
            _target = target;
        }

        public void Enter()
        {
            _navMeshAgent.destination = _target.Transform.position;
            _framesSkipped = 0;

            if (_navMeshAgent.isActiveAndEnabled)
                _navMeshAgent.isStopped = false;

            _animationPlayer.Play(_settings.AnimationTriggerName);
        }

        public void Exit()
        {
            if (_navMeshAgent.isActiveAndEnabled)
                _navMeshAgent.isStopped = true;
        }

        public void Tick(float deltaTime)
        {
            if (_enemy.IsWithinAttackTargetDistance())
                _stateMachine.Value.Enter<EnemyAttackingState>();
            else
                UpdateDestinationPosition();
        }

        private void UpdateDestinationPosition()
        {
            _framesSkipped++;
            if (_framesSkipped == _settings.UpdateFramesInterval)
            {
                _framesSkipped = 0;
                if (_navMeshAgent.isActiveAndEnabled)
                    _navMeshAgent.destination = _target.Transform.position;
            }
        }

        [Serializable]
        public class Settings
        {
            public string AnimationTriggerName;
            public int UpdateFramesInterval = 60;
        }
    }
}