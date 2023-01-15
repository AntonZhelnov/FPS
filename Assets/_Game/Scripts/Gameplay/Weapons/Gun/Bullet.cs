using System;
using Common.ParticleEffects;
using Gameplay.Damaging;
using Gameplay.Levels;
using UnityEngine;
using Zenject;

namespace Gameplay.Weapons.Gun
{
    public class Bullet : MonoBehaviour, IFixedTickable, IPoolable<DamageGroup, IMemoryPool>
    {
        [SerializeField] private Rigidbody _rigidbody;

        private int _damage;
        private DamageGroup _damageGroup;
        private float _lifeTime;
        private ParticleEffect.Factory _particleEffectFactory;
        private IMemoryPool _pool;
        private Settings _settings;
        private SignalBus _signalBus;
        private TickableManager _tickableManager;


        [Inject]
        public void Construct(
            Settings settings,
            int damage,
            ParticleEffect.Factory particleEffectFactory,
            TickableManager tickableManager,
            SignalBus signalBus)
        {
            _settings = settings;
            _damage = damage;
            _particleEffectFactory = particleEffectFactory;
            _tickableManager = tickableManager;
            _signalBus = signalBus;
        }

        public void Start()
        {
            _tickableManager.AddFixed(this);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out IDamageable damageable)
                && damageable.DamageGroup != _damageGroup)
            {
                damageable.ReceiveDamage(
                    _damage,
                    collision.contacts[0].point);
            }
            else
                _particleEffectFactory.Create(collision.contacts[0].point);

            Recycle();
        }

        public void FixedTick()
        {
            if (_pool is not null)
            {
                var deltaTime = Time.deltaTime;
                _lifeTime += deltaTime;
                if (_lifeTime < _settings.ExpirationTime)
                {
                    _rigidbody.MovePosition(
                        _settings.MovementSpeed * deltaTime * transform.forward + transform.position);
                }
                else
                    Recycle();
            }
        }

        public void OnDespawned()
        {
            _signalBus.Unsubscribe<LevelRestartingSignal>(Recycle);
            _pool = null;
        }

        public void OnSpawned(
            DamageGroup damageGroup,
            IMemoryPool pool)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _damageGroup = damageGroup;
            _lifeTime = 0f;
            _signalBus.Subscribe<LevelRestartingSignal>(Recycle);
            _pool = pool;
        }

        private void Recycle()
        {
            _pool?.Despawn(this);
        }

        public class Factory : PlaceholderFactory<DamageGroup, Bullet>
        {
        }

        [Serializable]
        public class Settings
        {
            [Min(.1f)] public float MovementSpeed;
            [Min(.1f)] public float ExpirationTime;
        }
    }
}