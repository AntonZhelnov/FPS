using Common;
using Configs.Weapons;
using Gameplay.Weapons;
using Gameplay.Weapons.Shooting;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GunInstaller : MonoInstaller
    {
        [Inject] private BulletConfig _bulletConfig;
        [Inject] private GunConfig _gunConfig;


        public override void InstallBindings()
        {
            Container.Bind<BulletEvents>().AsSingle();
            Container.BindInstance(_gunConfig.Damage)
                .WhenInjectedInto<Bullet>();

            Container.BindFactory<DamageGroup, Bullet, Bullet.Factory>()
                .FromPoolableMemoryPool<DamageGroup, Bullet, BulletPool>(
                    binder => binder
                        .WithInitialSize(10)
                        .FromComponentInNewPrefab(_bulletConfig.BulletPrefab)
                        .UnderTransformGroup("Bullets"));

            Container.BindFactory<DamageGroup, Transform, float, Shooter, Shooter.Factory>();
            Container.BindFactory<DamageGroup, Gun, Gun.Factory>()
                .FromComponentInNewPrefab(_gunConfig.WeaponPrefab);
        }

        private class BulletPool : MonoPoolableMemoryPool<DamageGroup, IMemoryPool, Bullet>
        {
        }
    }
}