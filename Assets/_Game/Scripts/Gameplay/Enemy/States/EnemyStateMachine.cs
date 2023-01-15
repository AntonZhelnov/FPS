using System;
using System.Collections.Generic;
using Common.States;
using Zenject;

namespace Gameplay.Enemy.States
{
    public class EnemyStateMachine : StateMachine
    {
        public EnemyStateMachine(
            TickableManager tickableManager,
            EnemyChasingState enemyChasingState,
            EnemyAttackingState enemyAttackingState,
            EnemyHurtState enemyHurtState,
            EnemyStunnedState enemyStunnedState,
            EnemyIdleState enemyIdleState,
            EnemyDeadState enemyDeadState)
            : base(tickableManager)
        {
            States = new Dictionary<Type, IState>
            {
                { typeof(EnemyChasingState), enemyChasingState },
                { typeof(EnemyAttackingState), enemyAttackingState },
                { typeof(EnemyHurtState), enemyHurtState },
                { typeof(EnemyStunnedState), enemyStunnedState },
                { typeof(EnemyIdleState), enemyIdleState },
                { typeof(EnemyDeadState), enemyDeadState }
            };
        }
    }
}