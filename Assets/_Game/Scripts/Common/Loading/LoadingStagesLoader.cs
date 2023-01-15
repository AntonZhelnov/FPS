using System.Collections.Generic;
using Common.Loading.Stages;
using Cysharp.Threading.Tasks;

namespace Common.Loading
{
    public class LoadingStagesLoader
    {
        private readonly LocalPrefabSpawner _localPrefabSpawner;
        private readonly SceneLoadingStage.Factory _sceneLoadingStageFactory;


        public LoadingStagesLoader(
            LocalPrefabSpawner localPrefabSpawner,
            SceneLoadingStage.Factory sceneLoadingStageFactory)
        {
            _localPrefabSpawner = localPrefabSpawner;
            _sceneLoadingStageFactory = sceneLoadingStageFactory;
        }

        public async UniTask Load(List<ILoadingStage> loadingStages)
        {
            var loadingScreen = await _localPrefabSpawner.Spawn<LoadingScreen>();

            foreach (var loadingStage in loadingStages)
            {
                loadingScreen.TrackLoadingStage(loadingStage);
                await loadingStage.Load();
            }

            await loadingScreen.WaitForProgressBarFill();
            _localPrefabSpawner.Release();
        }

        public void Load(SceneLoadingStageConfig sceneLoadingStageConfig)
        {
            Load(new List<ILoadingStage>
            {
                _sceneLoadingStageFactory.Create(sceneLoadingStageConfig)
            });
        }
    }
}