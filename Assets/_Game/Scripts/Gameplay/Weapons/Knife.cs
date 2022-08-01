using Common;
using UnityEngine;
using Zenject;

namespace Gameplay.Weapons
{
    public class Knife : Weapon
    {
        private IDamageable _damageable;


        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent(typeof(IDamageable)) is IDamageable damageable)
            {
                if (damageable.DamageGroup != DamageGroup)
                    _damageable = damageable;
            }
        }

        public override void Attack(Vector3 targetPosition)
        {
            if (_damageable != null)
            {
                _damageable.ReceiveDamage(Damage);
                _damageable = null;
            }
        }

        public class Factory : PlaceholderFactory<DamageGroup, Knife>
        {
        }
    }
}