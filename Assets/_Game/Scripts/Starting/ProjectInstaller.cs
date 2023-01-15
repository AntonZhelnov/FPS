using Common.Loading;
using Common.Loading.Stages;
using Common.Saving;
using Profile;
using UnityEngine;
using Zenject;

namespace Starting
{
    [CreateAssetMenu(
        fileName = "New Project",
        menuName = "Installers/Project")]
    public class ProjectInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private SceneLoadingStageConfig _menuSceneLoadingStageConfig;


        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.BindInstance(_menuSceneLoadingStageConfig);
            Container.Bind<LocalPrefabSpawner>().AsSingle();

            InstallLoading();
            InstallProfile();

            Container.BindInterfacesTo<ProjectStarter>().AsSingle().NonLazy();
        }

        private void InstallLoading()
        {
            Container.Bind<LoadingStagesLoader>().AsSingle();
            Container.Bind<AddressablesLoadingStage>().AsSingle();
            Container.BindInterfacesAndSelfTo<AddressablesProvider>().AsSingle();
            Container.BindFactory<SceneLoadingStageConfig, SceneLoadingStage, SceneLoadingStage.Factory>().AsSingle();
        }

        private void InstallProfile()
        {
            Container.BindInterfacesAndSelfTo<PlayerProfile>().AsSingle();
            Container.BindInterfacesTo<LocalDataProvider<PlayerProfileData>>().AsSingle().NonLazy();
            Container.Bind<PlayerProfileLoadingStage>().AsSingle();
        }
    }
}