using Common;
using Configs;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Gameplay.Enemies.States
{
    public class DeadEnemyState : EnemyState
    {
        private readonly Collider _collider;
        private readonly EnemyEvents _enemyEvents;
        private readonly NavMeshAgent _navMeshAgent;
        private Transform _enemyTransform;


        public DeadEnemyState(
            Enemy enemy,
            EnemyEvents enemyEvents,
            NavMeshAgent navMeshAgent,
            Collider collider,
            AnimationConfig animationConfig,
            AnimationPlayer.Factory animationPlayerFactory)
            : base(enemy, animationConfig, animationPlayerFactory)
        {
            _enemyEvents = enemyEvents;
            _navMeshAgent = navMeshAgent;
            _collider = collider;
        }

        public override void Initialize()
        {
            base.Initialize();
            _enemyTransform = Enemy.transform;
        }

        public override void Start()
        {
            _navMeshAgent.destination = _enemyTransform.position;
            _collider.enabled = false;
            AnimationPlayer.Play();
            _enemyEvents.OnDied(Enemy);
        }

        protected override void AnimationPlayerCallback()
        {
            Enemy.Recycle();
        }

        public class Factory : PlaceholderFactory<Enemy, EnemyEvents, NavMeshAgent, Collider, AnimationConfig,
            DeadEnemyState>
        {
        }
    }
}