using System;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Common.Loading.Stages
{
    public class AddressablesLoadingStage : ILoadingStage
    {
        public event Action<float> Loaded;
        public string Description => "Assets initialization...";


        public async UniTask Load()
        {
            Loaded?.Invoke(.5f);

            var handle = Addressables.InitializeAsync();
            await handle.Task;

            Loaded?.Invoke(1f);
        }
    }
}