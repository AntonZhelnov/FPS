using System;
using Common.Loading;
using Common.Loading.Stages;
using Gameplay.Levels;
using Zenject;

namespace Menu
{
    public class MenuLoader : IInitializable, IDisposable
    {
        private readonly LoadingStagesLoader _loadingStagesLoader;
        private readonly SceneLoadingStageConfig _menuSceneLoadingStageConfig;
        private readonly SignalBus _signalBus;


        public MenuLoader(
            LoadingStagesLoader loadingStagesLoader,
            SceneLoadingStageConfig sceneLoadingStageConfig,
            SignalBus signalBus)
        {
            _loadingStagesLoader = loadingStagesLoader;
            _menuSceneLoadingStageConfig = sceneLoadingStageConfig;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<LevelExitCommand>(LoadMenu);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<LevelExitCommand>(LoadMenu);
        }

        private void LoadMenu()
        {
            _loadingStagesLoader.Load(_menuSceneLoadingStageConfig);
        }
    }
}