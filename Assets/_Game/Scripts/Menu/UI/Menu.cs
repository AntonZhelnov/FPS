using System;
using Common.Loading;
using Common.Loading.Stages;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Menu.UI
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private MenuItem[] _menuItems;

        private LoadingStagesLoader _loadingStagesLoader;


        [Inject]
        public void Construct(LoadingStagesLoader loadingStagesLoader)
        {
            _loadingStagesLoader = loadingStagesLoader;
        }

        public void Start()
        {
            foreach (var menuItem in _menuItems)
                menuItem.Button.OnClickAsObservable()
                    .Subscribe(_ => _loadingStagesLoader.Load(menuItem.LoadingStageConfig))
                    .AddTo(this);
        }
    }

    [Serializable]
    public class MenuItem
    {
        [SerializeField] private Button _button;
        [SerializeField] private SceneLoadingStageConfig _sceneLoadingStageConfig;

        public Button Button => _button;
        public SceneLoadingStageConfig LoadingStageConfig => _sceneLoadingStageConfig;
    }
}