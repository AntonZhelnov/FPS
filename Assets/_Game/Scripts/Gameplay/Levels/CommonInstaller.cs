using Common.Pausing;
using Input;
using Menu;
using Profile;
using UnityEngine;
using Zenject;

namespace Gameplay.Levels
{
    [CreateAssetMenu(
        fileName = "New Common",
        menuName = "Installers/Levels/Common")]
    public class CommonInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<PoolCleanupChecker>().AsSingle();
            Container.Bind<Pauser>().AsSingle();
            Container.BindInterfacesTo<TickableManagerPauser>().AsSingle().NonLazy();
            Container.BindInterfacesTo<MenuLoader>().AsSingle().NonLazy();

            InstallLevelStateCommands();
            InstallLevelSignals();

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            Container.BindInterfacesTo<MouseLockSwitcher>().AsSingle().NonLazy();
#endif
        }

        private void InstallLevelSignals()
        {
            Container.DeclareSignal<LevelStartedSignal>().OptionalSubscriber();
            Container.DeclareSignal<LevelLostSignal>();
            Container.DeclareSignal<LevelRestartingSignal>();
        }

        private void InstallLevelStateCommands()
        {
            Container.DeclareSignal<LevelExitCommand>();
            Container.DeclareSignal<LevelPauseCommand>();
            Container.DeclareSignal<LevelRestartCommand>();
            Container.DeclareSignal<LevelResumeCommand>();

            Container.BindSignal<LevelRestartCommand>()
                .ToMethod<PlayerProfile>(playerProfile => playerProfile.Save).FromResolve();

            Container.BindSignal<LevelExitCommand>()
                .ToMethod<PlayerProfile>(playerProfile => playerProfile.Save).FromResolve();
        }
    }
}