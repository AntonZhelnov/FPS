using Gameplay.Weapons;
using UnityEngine;

namespace Configs.Weapons
{
    [CreateAssetMenu(
        fileName = "New Knife Config Installer",
        menuName = "Configs/Weapons/Knife Config Installer")]
    public class KnifeConfig : WeaponConfig
    {
        public override void InstallBindings()
        {
            Container.Bind<WeaponConfig>()
                .FromInstance(this).AsCached()
                .WhenInjectedInto<Knife>();

            Container.BindInstance(this).AsSingle();
        }
    }
}