using Common;
using Configs.Weapons;
using UnityEngine;
using Zenject;

namespace Gameplay.Weapons.Shooting
{
    public class Bullet : MonoBehaviour, ITickable, IPoolable<DamageGroup, IMemoryPool>, IInitializable
    {
        private BulletConfig _bulletConfig;
        private BulletEvents _bulletEvents;
        private int _damage;
        private float _expirationTime;
        private float _lifeTime;
        private IMemoryPool _pool;
        private float _speed;
        private TickableManager _tickableManager;


        [Inject]
        public void Construct(
            BulletConfig bulletConfig,
            BulletEvents bulletEvents,
            int damage,
            TickableManager tickableManager)
        {
            _bulletConfig = bulletConfig;
            _bulletEvents = bulletEvents;
            _damage = damage;
            _tickableManager = tickableManager;
        }

        [Inject]
        public void Initialize()
        {
            _tickableManager.Add(this);
            _speed = _bulletConfig.BulletMovementSpeed;
            _expirationTime = _bulletConfig.BulletExpirationTime;
        }

        private DamageGroup DamageGroup { get; set; }


        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent(typeof(IDamageable)) is IDamageable damageable
                && damageable.DamageGroup != DamageGroup)
            {
                damageable.ReceiveDamage(_damage);
                _bulletEvents.OnHit(this);
            }

            Recycle();
        }

        public void OnDespawned()
        {
            _pool = null;
        }

        public void OnSpawned(
            DamageGroup damageGroup,
            IMemoryPool pool)
        {
            DamageGroup = damageGroup;
            _lifeTime = 0f;
            _pool = pool;
        }

        public void Tick()
        {
            if (_pool != null)
            {
                var deltaTime = Time.deltaTime;
                _lifeTime += deltaTime;
                if (_lifeTime < _expirationTime)
                    transform.Translate(_speed * deltaTime * Vector3.forward);
                else
                    Recycle();
            }
        }

        private void Recycle()
        {
            _pool.Despawn(this);
        }

        public class Factory : PlaceholderFactory<DamageGroup, Bullet>
        {
        }
    }
}