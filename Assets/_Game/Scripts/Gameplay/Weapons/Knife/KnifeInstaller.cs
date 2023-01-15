using Common.Loading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Gameplay.Weapons.Knife
{
    [CreateAssetMenu(
        fileName = "New Knife",
        menuName = "Installers/Weapons/Knife")]
    public class KnifeInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private Weapon.Settings _knifeSettings;
        [SerializeField] private Knife.HitSettings _knifeHitSettings;
        [SerializeField] private AssetReferenceGameObject _knife;

        [Inject] private readonly AddressablesProvider _addressablesProvider;


        public override void InstallBindings()
        {
            Container.Bind<Weapon.Settings>()
                .FromInstance(_knifeSettings).AsSingle()
                .WhenInjectedInto<Knife>();

            Container.Bind<Knife.HitSettings>()
                .FromInstance(_knifeHitSettings).AsSingle()
                .WhenInjectedInto<Knife>();

            Container.BindFactory<Knife, Knife.Factory>()
                .FromPoolableMemoryPool<Knife, Pool>(
                    binder => binder
                        .WithInitialSize(10)
                        .FromComponentInNewPrefab(_addressablesProvider.Get<GameObject>(_knife))
                        .UnderTransformGroup("Knives"));
        }

        private class Pool : MonoPoolableMemoryPool<IMemoryPool, Knife>
        {
        }
    }
}