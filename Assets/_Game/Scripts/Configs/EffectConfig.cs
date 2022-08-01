using Common.Effects;
using UnityEngine;
using Zenject;

namespace Configs
{
    [CreateAssetMenu(
        fileName = "New Effect Config Installer",
        menuName = "Configs/Effect Config Installer")]
    public class EffectConfig : ScriptableObjectInstaller
    {
        [SerializeField] private Effect _effectPrefab;
        [SerializeField] [Min(0.1f)] private float _expirationTime = 1f;

        public Effect EffectPrefab => _effectPrefab;
        public float ExpirationTime => _expirationTime;


        public override void InstallBindings()
        {
            Container.BindInstance(this).AsSingle();
        }
    }
}