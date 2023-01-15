using Gameplay.Levels.States;
using Gameplay.Weapons;
using UnityEngine;
using Zenject;

namespace Gameplay.Levels.Prototype
{
    [CreateAssetMenu(
        fileName = "New Prototype",
        menuName = "Installers/Levels/Prototype")]
    public class PrototypeLevelInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<RandomWeaponProvider>().AsSingle();

            InstallStates();

            Container.BindInterfacesTo<PrototypeLevelStarter>().AsSingle();
        }

        private void InstallStates()
        {
            Container.BindInterfacesAndSelfTo<LevelStateMachine>().AsSingle();
            Container.Bind<LevelStartingState>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelPlayingState>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelPausedState>().AsSingle();
            Container.Bind<LevelLostState>().AsSingle();
            Container.Bind<LevelRestartingState>().AsSingle();
        }
    }
}