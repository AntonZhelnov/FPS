using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Common.Loading
{
    public class AddressablesProvider : IDisposable
    {
        private readonly List<AsyncOperationHandle> _assetHandles = new();
        private readonly Dictionary<string, object> _assets = new();


        public void Dispose()
        {
            ReleaseAssets();
        }

        public T Get<T>(string key)
        {
            if (!_assets.TryGetValue(key, out var asset))
                return default;

            return
                asset is T t
                    ? t
                    : default;
        }

        public T Get<T>(AssetReference assetReference)
        {
            var key = assetReference.RuntimeKey.ToString();
            return Get<T>(key);
        }

        public async UniTask Load<T>(string key)
        {
            if (_assets.ContainsKey(key))
                return;

            try
            {
                var assetHandle = Addressables.LoadAssetAsync<T>(key);
                _assetHandles.Add(assetHandle);

                var asset = await assetHandle.ToUniTask();
                _assets.Add(key, asset);
            }
            catch (InvalidKeyException e)
            {
                Debug.LogException(e);
            }
        }

        public async UniTask Load<T>(AssetReference assetReference)
        {
            var key = assetReference.RuntimeKey.ToString();
            await Load<T>(key);
        }

        public void ReleaseAssets()
        {
            foreach (var assetHandle in _assetHandles)
                Addressables.Release(assetHandle);
            _assetHandles.Clear();
        }
    }
}