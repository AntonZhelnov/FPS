using System;
using Common.Physics;
using Common.States;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Gameplay.Enemy.States
{
    public class EnemyDeadState : IState
    {
        private readonly CapsuleCollider _collider;
        private readonly Enemy _enemy;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly RagdollActivator _ragdollActivator;
        private readonly Settings _settings;
        private readonly SignalBus _signalBus;
        private float _timePassed;


        public EnemyDeadState(
            Settings settings,
            Enemy enemy,
            NavMeshAgent navMeshAgent,
            RagdollActivator ragdollActivator,
            CapsuleCollider collider,
            SignalBus signalBus)
        {
            _settings = settings;
            _enemy = enemy;
            _navMeshAgent = navMeshAgent;
            _collider = collider;
            _ragdollActivator = ragdollActivator;
            _signalBus = signalBus;
        }

        public void Enter()
        {
            _timePassed = 0f;
            _navMeshAgent.isStopped = true;
            _collider.enabled = false;
            _ragdollActivator.Activate();
            _signalBus.Fire(new EnemyDiedSignal(_enemy));
        }

        public void Exit()
        {
            _navMeshAgent.isStopped = false;
            _ragdollActivator.Deactivate();
            _collider.enabled = true;
        }

        public void Tick(float deltaTime)
        {
            _timePassed += Time.deltaTime;
            if (_timePassed >= _settings.ActionDelay)
                _enemy.Recycle();
        }

        [Serializable]
        public class Settings
        {
            [Min(0f)] public float ActionDelay;
        }
    }
}