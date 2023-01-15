using Common.Loading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Common.ParticleEffects
{
    [CreateAssetMenu(
        fileName = "New Particle Effects",
        menuName = "Installers/Particle Effects")]
    public class ParticleEffectsInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private AssetReferenceGameObject[] _particleEffects;

        [Inject] private readonly AddressablesProvider _addressablesProvider;


        public override void InstallBindings()
        {
            foreach (var particleEffect in _particleEffects)
                Container.BindFactory<Vector3, ParticleEffect, ParticleEffect.Factory>()
                    .WithId(particleEffect.RuntimeKey)
                    .FromPoolableMemoryPool<Vector3, ParticleEffect, ParticleEffectPool>(
                        binder => binder
                            .WithInitialSize(10)
                            .FromComponentInNewPrefab(_addressablesProvider.Get<GameObject>(particleEffect))
                            .UnderTransformGroup("ParticleEffects"));
        }

        private class ParticleEffectPool : MonoPoolableMemoryPool<Vector3, IMemoryPool, ParticleEffect>
        {
        }
    }
}