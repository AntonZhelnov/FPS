using Common.Pausing;
using UnityEngine;
using Zenject;

namespace Common.Effects
{
    public class Effect : MonoBehaviour, IPausable, ITickable, IPoolable<IMemoryPool>, IInitializable
    {
        [SerializeField] private ParticleSystem _particleSystem;

        private float _expirationTime;
        private float _lifeTime;
        private Pauser _pauser;
        private IMemoryPool _pool;
        private TickableManager _tickableManager;


        [Inject]
        public void Construct(
            Pauser pauser,
            float expirationTime,
            TickableManager tickableManager)
        {
            _pauser = pauser;
            _expirationTime = expirationTime;
            _tickableManager = tickableManager;
        }

        [Inject]
        public void Initialize()
        {
            _tickableManager.Add(this);
        }

        public void Pause()
        {
            _particleSystem.Pause();
        }

        public void Resume()
        {
            _particleSystem.Play();
        }

        public void OnDespawned()
        {
            _pool = null;
            _pauser.Unregister(this);
        }

        public void OnSpawned(IMemoryPool pool)
        {
            _pauser.Register(this);
            _lifeTime = 0f;
            _pool = pool;
        }

        public void Tick()
        {
            if (_pool != null)
            {
                _lifeTime += Time.deltaTime;
                if (_lifeTime > _expirationTime)
                    _pool.Despawn(this);
            }
        }

        public class Factory : PlaceholderFactory<Effect>
        {
        }
    }
}