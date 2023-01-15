using UnityEngine;

namespace Gameplay.Damaging
{
    public interface IDamageable
    {
        public DamageGroup DamageGroup { get; }


        public void ReceiveDamage(
            int damage,
            Vector3 position);
    }
}