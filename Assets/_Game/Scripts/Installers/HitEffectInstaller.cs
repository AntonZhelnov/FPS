using Common.Effects;
using Configs;
using Zenject;

namespace Installers
{
    public class HitEffectInstaller : MonoInstaller
    {
        [Inject] private EffectConfig _hitEffectConfig;


        public override void InstallBindings()
        {
            Container.BindInstance(_hitEffectConfig.ExpirationTime).WhenInjectedInto<Effect>();

            Container.BindFactory<Effect, Effect.Factory>()
                .FromPoolableMemoryPool<Effect, EffectPool>(
                    binder => binder
                        .WithInitialSize(10)
                        .FromComponentInNewPrefab(_hitEffectConfig.EffectPrefab)
                        .UnderTransformGroup("Effects"));

            Container.Bind<EffectSpawner>().AsSingle();
        }

        private class EffectPool : MonoPoolableMemoryPool<IMemoryPool, Effect>
        {
        }
    }
}