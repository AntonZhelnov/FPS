using Gameplay.Weapons;
using UnityEngine;

namespace Configs.Weapons
{
    [CreateAssetMenu(
        fileName = "New Gun Config Installer",
        menuName = "Configs/Weapons/Gun Config Installer")]
    public class GunConfig : WeaponConfig
    {
        public override void InstallBindings()
        {
            Container.Bind<WeaponConfig>()
                .FromInstance(this).AsCached()
                .WhenInjectedInto<Gun>();

            Container.BindInstance(this).AsSingle();
        }
    }
}