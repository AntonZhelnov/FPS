using Common;
using Common.ParticleEffects;
using UnityEngine;

namespace Gameplay.Damaging
{
    public class DamageReactor
    {
        private readonly Health _health;
        private readonly ParticleEffect.Factory _particleEffectFactory;


        public DamageReactor(
            Health health,
            ParticleEffect.Factory particleEffectFactory)
        {
            _health = health;
            _particleEffectFactory = particleEffectFactory;
        }

        public void React(
            int damage,
            Vector3 position)
        {
            _health.Decrease(damage);
            _particleEffectFactory.Create(position);
        }
    }
}