using System;
using Common;
using Common.Animating;
using Common.Loading;
using Common.ParticleEffects;
using Common.Pausing;
using Gameplay.Damaging;
using Gameplay.Enemy.States;
using Gameplay.Weapons;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Gameplay.Enemy
{
    [CreateAssetMenu(
        fileName = "New Enemy",
        menuName = "Installers/Enemy")]
    public class EnemyInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private Enemy.Settings _enemySettings;
        [SerializeField] private EnemyStates _enemyStatesSettings;
        [SerializeField] private EnemySpawner.Settings _enemySpawnerSettings;
        [SerializeField] private EnemySpawnPoint.Settings _enemySpawnPointSettings;
        [SerializeField] private AssetReferenceGameObject _enemy;
        [SerializeField] private AssetReferenceGameObject _hitEffect;

        [Inject] private readonly AddressablesProvider _addressablesProvider;


        public override void InstallBindings()
        {
            Container.Bind<EnemyTarget>().AsSingle();

            Container.BindFactory<Enemy, Enemy.Factory>()
                .FromPoolableMemoryPool<Enemy, Pool>(
                    binder => binder
                        .WithInitialSize(_enemySpawnerSettings.EnemiesMaximum)
                        .FromSubContainerResolve()
                        .ByNewPrefabMethod(
                            _addressablesProvider.Get<GameObject>(_enemy),
                            InstallEnemy)
                        .UnderTransformGroup("Enemies"));

            Container.BindInterfacesAndSelfTo<EnemySpawner>().AsSingle()
                .WithArguments(_enemySpawnerSettings);

            Container.BindInstance(_enemySpawnPointSettings)
                .WhenInjectedInto<EnemySpawnPoint>();

            Container.DeclareSignal<EnemyDiedSignal>();
        }

        private void InstallEnemy(DiContainer subContainer)
        {
            subContainer.Bind<Enemy>().FromComponentOnRoot().AsSingle();
            subContainer.BindInstance(_enemySettings).AsSingle();

            subContainer.BindInterfacesTo<TickableManagerPauser>().AsSingle().NonLazy();
            subContainer.BindInterfacesTo<AnimatorPauser>().AsSingle().NonLazy();
            subContainer.BindInterfacesTo<NavMeshAgentPauser>().AsSingle().NonLazy();
            subContainer.BindInterfacesAndSelfTo<AnimationPlayer>().AsSingle();

            subContainer.Bind<Health>().AsSingle()
                .WithArguments(_enemySettings.Health);
            subContainer.BindInstance(DamageGroup.Enemy).AsSingle();
            subContainer.Bind<DamageReactor>().AsSingle();
            subContainer.BindInstance(Container.ResolveId<ParticleEffect.Factory>(_hitEffect.RuntimeKey)).AsSingle();

            subContainer.Bind<WeaponHolder>().AsSingle();

            subContainer.BindInterfacesAndSelfTo<EnemyStateMachine>().AsSingle();

            subContainer.Bind<EnemyChasingState>().AsSingle()
                .WithArguments(_enemyStatesSettings.ChasingSettings);
            subContainer.Bind<EnemyAttackingState>().AsSingle()
                .WithArguments(_enemyStatesSettings.AttackingSettings);
            subContainer.Bind<EnemyHurtState>().AsSingle()
                .WithArguments(_enemyStatesSettings.HurtSettings);
            subContainer.Bind<EnemyStunnedState>().AsSingle()
                .WithArguments(_enemyStatesSettings.StunSettings);
            subContainer.Bind<EnemyIdleState>().AsSingle()
                .WithArguments(_enemyStatesSettings.IdleSettings);
            subContainer.Bind<EnemyDeadState>().AsSingle()
                .WithArguments(_enemyStatesSettings.DeadSettings);
        }

        private class Pool : MonoPoolableMemoryPool<IMemoryPool, Enemy>
        {
        }

        [Serializable]
        public class EnemyStates
        {
            public EnemyIdleState.Settings IdleSettings;
            public EnemyChasingState.Settings ChasingSettings;
            public EnemyAttackingState.Settings AttackingSettings;
            public EnemyHurtState.Settings HurtSettings;
            public EnemyStunnedState.Settings StunSettings;
            public EnemyDeadState.Settings DeadSettings;
        }
    }
}