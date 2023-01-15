using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Common.Loading.Stages
{
    public class SceneLoadingStage : ILoadingStage
    {
        public event Action<float> Loaded;

        private readonly AddressablesProvider _addressablesProvider;
        private readonly SceneLoadingStageConfig _sceneLoadingStageConfig;
        private int _loadedElementsCount;
        private int _loadingElementsCount;


        public SceneLoadingStage(
            SceneLoadingStageConfig sceneLoadingStageConfig,
            AddressablesProvider addressablesProvider)
        {
            _sceneLoadingStageConfig = sceneLoadingStageConfig;
            _addressablesProvider = addressablesProvider;
        }

        public string Description => _sceneLoadingStageConfig.Description;


        public async UniTask Load()
        {
            Loaded?.Invoke(.1f);

            _loadingElementsCount = _sceneLoadingStageConfig.GameObjectsAssetReferences.Count + 1;

            foreach (var assetReference in _sceneLoadingStageConfig.GameObjectsAssetReferences)
            {
                await _addressablesProvider.Load<GameObject>(assetReference);
                UpdateProgress(Loaded);
            }

            var operation = Addressables.LoadSceneAsync(_sceneLoadingStageConfig.SceneAssetReference);
            await operation.ToUniTask();

            Loaded?.Invoke(1f);
        }

        private void UpdateProgress(Action<float> onProgress)
        {
            _loadedElementsCount++;
            var progress = (float)_loadedElementsCount / _loadingElementsCount;
            onProgress?.Invoke(progress);
        }

        public class Factory : PlaceholderFactory<SceneLoadingStageConfig, SceneLoadingStage>
        {
        }
    }
}