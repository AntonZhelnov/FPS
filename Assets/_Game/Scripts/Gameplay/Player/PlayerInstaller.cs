using Common;
using Common.ParticleEffects;
using Gameplay.Damaging;
using Gameplay.Player.Abilities;
using Input;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Gameplay.Player
{
    [CreateAssetMenu(
        fileName = "New Player",
        menuName = "Installers/Player")]
    public class PlayerInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private Player.Settings _playerSettings;
        [SerializeField] private PlayerControlSettings _playerControlSettings;
        [SerializeField] private AssetReferenceGameObject _hitEffect;


        public override void InstallBindings()
        {
            Container.Bind<Controls>().AsSingle();
            Container.BindInstance(_playerSettings).AsSingle();

            Container.BindFactory<Player, Player.Factory>()
                .FromSubContainerResolve()
                .ByNewPrefabMethod(
                    _playerSettings.Prefab,
                    InstallPlayer);

            InstallPlayerSignals();
        }

        private void InstallPlayer(DiContainer subContainer)
        {
            subContainer.Bind<Player>().FromComponentOnRoot().AsSingle();

            subContainer.Bind<Health>().AsSingle()
                .WithArguments(_playerSettings.Health);
            subContainer.BindInstance(DamageGroup.Player);
            subContainer.Bind<DamageReactor>().AsSingle();
            subContainer.BindInstance(Container.ResolveId<ParticleEffect.Factory>(_hitEffect.RuntimeKey)).AsSingle();

            subContainer.BindInterfacesAndSelfTo<PlayerWeaponHolder>().AsSingle();

            subContainer.BindInterfacesTo<PlayerLooker>().AsSingle()
                .WithArguments(_playerControlSettings).NonLazy();
            subContainer.BindInterfacesTo<PlayerMover>().AsSingle()
                .WithArguments(_playerControlSettings).NonLazy();
            subContainer.BindInterfacesTo<PlayerAttacker>().AsSingle().NonLazy();
        }

        private void InstallPlayerSignals()
        {
            Container.DeclareSignal<PlayerSpawnedSignal>();
            Container.DeclareSignal<PlayerDiedSignal>();
        }
    }
}