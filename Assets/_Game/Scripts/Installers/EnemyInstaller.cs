using Common;
using Configs;
using Configs.Enemies;
using Gameplay.Enemies;
using Gameplay.Enemies.States;
using Gameplay.Weapons;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Installers
{
    public class EnemyInstaller : MonoInstaller
    {
        [Inject] private EnemySpawnConfig _enemySpawnConfig;


        public override void InstallBindings()
        {
            Container.Bind<EnemyEvents>().AsSingle();
            Container.Bind<EnemiesCommander>().AsSingle();
            Container.BindInstance(DamageGroup.Enemy).WhenInjectedInto<Enemy>();
            Container.BindInstance(_enemySpawnConfig.EnemyConfig).WhenInjectedInto<Enemy>();

            Container.BindFactory<Enemy, Enemy.Factory>()
                .FromPoolableMemoryPool<Enemy, EnemyPool>(
                    binder => binder
                        .WithInitialSize(_enemySpawnConfig.EnemiesMaximum)
                        .FromComponentInNewPrefab(_enemySpawnConfig.EnemyConfig.EnemyPrefab)
                        .UnderTransformGroup("Enemies"));

            Container.BindInterfacesAndSelfTo<InvisibleByPlayerPositionGenerator>().AsSingle()
                .WithArguments(_enemySpawnConfig.EnemySpawnAreaConfig);

            Container.BindInterfacesAndSelfTo<EnemySpawner>().AsSingle()
                .WithArguments(_enemySpawnConfig);

            Container.BindFactory<Enemy, NavMeshAgent, Transform, AnimationConfig, ChasingPlayerEnemyState,
                ChasingPlayerEnemyState.Factory>();
            Container.BindFactory<Enemy, Transform, Weapon, float, AnimationConfig, AttackingPlayerEnemyState,
                AttackingPlayerEnemyState.Factory>();
            Container.BindFactory<Enemy, AnimationConfig, HurtEnemyState, HurtEnemyState.Factory>();
            Container.BindFactory<Enemy, AnimationConfig, StunEnemyState, StunEnemyState.Factory>();
            Container.BindFactory<Enemy, AnimationConfig, IdleEnemyState, IdleEnemyState.Factory>();
            Container.BindFactory<Enemy, EnemyEvents, NavMeshAgent, Collider, AnimationConfig,
                DeadEnemyState, DeadEnemyState.Factory>();
        }

        private class EnemyPool : MonoPoolableMemoryPool<IMemoryPool, Enemy>
        {
        }
    }
}