using UnityEngine;

namespace Common.Effects
{
    public class EffectSpawner
    {
        private readonly Effect.Factory _effectFactory;


        public EffectSpawner(Effect.Factory effectFactory)
        {
            _effectFactory = effectFactory;
        }

        public void Spawn(Vector3 position)
        {
            var effect = _effectFactory.Create();
            effect.transform.position = position;
        }
    }
}