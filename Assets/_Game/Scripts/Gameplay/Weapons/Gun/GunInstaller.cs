using Common.Loading;
using Common.ParticleEffects;
using Common.Pausing;
using Gameplay.Damaging;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Gameplay.Weapons.Gun
{
    [CreateAssetMenu(
        fileName = "New Gun",
        menuName = "Installers/Weapons/Gun")]
    public class GunInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private Weapon.Settings _gunSettings;
        [SerializeField] private Bullet.Settings _bulletSettings;
        [SerializeField] private AssetReferenceGameObject _gun;
        [SerializeField] private AssetReferenceGameObject _bullet;
        [SerializeField] private AssetReferenceGameObject _bulletHitEffect;

        [Inject] private readonly AddressablesProvider _addressablesProvider;


        public override void InstallBindings()
        {
            Container.BindFactory<DamageGroup, Bullet, Bullet.Factory>()
                .FromPoolableMemoryPool<DamageGroup, Bullet, BulletPool>(
                    binder => binder
                        .WithInitialSize(10)
                        .FromSubContainerResolve()
                        .ByNewPrefabMethod(
                            _addressablesProvider.Get<GameObject>(_bullet),
                            InstallBullet)
                        .UnderTransformGroup("Bullets"));

            Container.BindFactory<Transform, float, Shooter, Shooter.Factory>();

            Container.Bind<Weapon.Settings>()
                .FromInstance(_gunSettings).AsCached()
                .WhenInjectedInto<Gun>();

            Container.BindFactory<Gun, Gun.Factory>()
                .FromPoolableMemoryPool<Gun, GunPool>(
                    binder => binder
                        .WithInitialSize(10)
                        .FromComponentInNewPrefab(_addressablesProvider.Get<GameObject>(_gun))
                        .UnderTransformGroup("Guns"));
        }

        private void InstallBullet(DiContainer subContainer)
        {
            subContainer.Bind<Bullet>().FromComponentOnRoot().AsSingle();

            subContainer.BindInterfacesTo<TickableManagerPauser>().AsSingle().NonLazy();
            subContainer.BindInstance(_bulletSettings).AsSingle();
            subContainer.BindInstance(_gunSettings.Damage);

            subContainer.BindInstance(Container.ResolveId<ParticleEffect.Factory>(_bulletHitEffect.RuntimeKey))
                .AsSingle();
        }

        private class BulletPool : MonoPoolableMemoryPool<DamageGroup, IMemoryPool, Bullet>
        {
        }

        private class GunPool : MonoPoolableMemoryPool<IMemoryPool, Gun>
        {
        }
    }
}