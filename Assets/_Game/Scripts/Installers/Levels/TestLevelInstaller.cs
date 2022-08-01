using Starters;
using Zenject;

namespace Installers.Levels
{
    public class TestLevelInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<TestLevelStarter>().AsSingle();
        }
    }
}