using Common;
using Configs;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Gameplay.Enemies.States
{
    public class ChasingPlayerEnemyState : EnemyState
    {
        private const int FramesInterval = 30;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly Transform _playerTransform;
        private int _framesSkipped;


        public ChasingPlayerEnemyState(
            Enemy enemy,
            NavMeshAgent navMeshAgent,
            Transform playerTransform,
            AnimationConfig animationConfig,
            AnimationPlayer.Factory animationPlayerFactory)
            : base(enemy, animationConfig, animationPlayerFactory)
        {
            _navMeshAgent = navMeshAgent;
            _playerTransform = playerTransform;
        }

        public override void Start()
        {
            _framesSkipped = 0;
            if (_navMeshAgent.isActiveAndEnabled)
                _navMeshAgent.isStopped = false;
            AnimationPlayer.Play();
        }

        public override void Stop()
        {
            if (_navMeshAgent.isActiveAndEnabled)
                _navMeshAgent.isStopped = true;
            AnimationPlayer.Stop();
        }

        public override void UpdateTime(float deltaTime)
        {
            if (Enemy.IsWithinAttackRange())
                StateMachine.SwitchState<AttackingPlayerEnemyState>();
            else
                UpdateDestinationPosition();
        }

        private void UpdateDestinationPosition()
        {
            _framesSkipped++;
            if (_framesSkipped == FramesInterval)
            {
                _framesSkipped = 0;
                if (_navMeshAgent.isActiveAndEnabled)
                    _navMeshAgent.destination = _playerTransform.position;
            }
        }

        public class Factory : PlaceholderFactory<Enemy, NavMeshAgent, Transform, AnimationConfig,
            ChasingPlayerEnemyState>
        {
        }
    }
}