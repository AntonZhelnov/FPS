using System.Collections.Generic;
using Common.Loading;
using Common.Loading.Stages;
using Profile;
using Zenject;

namespace Starting
{
    public class ProjectStarter : IInitializable
    {
        private readonly AddressablesLoadingStage _addressablesLoadingStage;
        private readonly LoadingStagesLoader _loadingStagesLoader;
        private readonly SceneLoadingStageConfig _menuSceneLoadingStageConfig;
        private readonly PlayerProfileLoadingStage _playerProfileLoadingStage;
        private readonly SceneLoadingStage.Factory _sceneLoadingStageFactory;


        public ProjectStarter(
            PlayerProfileLoadingStage playerProfileLoadingStage,
            AddressablesLoadingStage addressablesLoadingStage,
            LoadingStagesLoader loadingStagesLoader,
            SceneLoadingStage.Factory sceneLoadingStageFactory,
            SceneLoadingStageConfig loadingStageConfig)
        {
            _playerProfileLoadingStage = playerProfileLoadingStage;
            _addressablesLoadingStage = addressablesLoadingStage;
            _loadingStagesLoader = loadingStagesLoader;
            _sceneLoadingStageFactory = sceneLoadingStageFactory;
            _menuSceneLoadingStageConfig = loadingStageConfig;
        }

        public void Initialize()
        {
            var menuSceneLoadingStage = _sceneLoadingStageFactory.Create(_menuSceneLoadingStageConfig);

            _loadingStagesLoader.Load(new List<ILoadingStage>
            {
                _playerProfileLoadingStage,
                _addressablesLoadingStage,
                menuSceneLoadingStage
            });
        }
    }
}