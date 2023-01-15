using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Common.Loading
{
    public class LocalPrefabSpawner
    {
        private GameObject _gameObject;


        public void Release()
        {
            if (!_gameObject)
                return;

            _gameObject.SetActive(false);
            Addressables.ReleaseInstance(_gameObject);
            _gameObject = null;
        }

        public async UniTask<T> Spawn<T>() where T : MonoBehaviour
        {
            var handle = Addressables.InstantiateAsync(typeof(T).Name);
            _gameObject = await handle.Task;

            if (_gameObject.TryGetComponent(out T component) is false)
                throw new NullReferenceException($"{typeof(T)} not loaded");

            return component;
        }
    }
}