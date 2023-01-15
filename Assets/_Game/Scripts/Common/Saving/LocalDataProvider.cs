using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Saving
{
    public class LocalDataProvider<T> : IDataProvider<T> where T : new()
    {
        private readonly string _filePath;
        private T _data;


        public LocalDataProvider()
        {
            _filePath = $"{Application.persistentDataPath}/{typeof(T).Name}.dat";
        }

        public void Save()
        {
            using var file = File.Create(_filePath);
            new BinaryFormatter().Serialize(file, _data);
        }

        public async UniTask<T> Load()
        {
            if (File.Exists(_filePath))
            {
                await using var fileStream =
                    new FileStream(
                        _filePath,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.None,
                        4096,
                        true);

                var loadedSaveData = new BinaryFormatter().Deserialize(fileStream);
                _data = (T)loadedSaveData;
            }
            else
                _data = new T();

            return _data;
        }
    }
}