using System.Collections.Generic;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Common.Loading.Stages
{
    [CreateAssetMenu(
        fileName = "New Scene",
        menuName = "Loading Stages/Scene")]
    public class SceneLoadingStageConfig : ScriptableObject
    {
        [SerializeField] private SceneReference _scene;
        [SerializeField] private string _description = "Loading...";
        [SerializeField] private List<AssetReferenceGameObject> _gameObjects;


        public string SceneAssetReference => _scene.AssetGuidHex;
        public string Description => _description;
        public List<AssetReferenceGameObject> GameObjectsAssetReferences => _gameObjects;
    }
}