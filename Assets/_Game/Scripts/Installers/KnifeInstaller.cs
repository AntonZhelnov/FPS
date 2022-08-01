using Common;
using Configs.Weapons;
using Gameplay.Weapons;
using Zenject;

namespace Installers
{
    public class KnifeInstaller : MonoInstaller
    {
        [Inject] private KnifeConfig _knifeConfig;


        public override void InstallBindings()
        {
            Container.BindFactory<DamageGroup, Knife, Knife.Factory>()
                .FromComponentInNewPrefab(_knifeConfig.WeaponPrefab);
        }
    }
}