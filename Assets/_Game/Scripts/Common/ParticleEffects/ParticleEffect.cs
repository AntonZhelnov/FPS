using Common.Pausing;
using UnityEngine;
using Zenject;

namespace Common.ParticleEffects
{
    public class ParticleEffect : MonoBehaviour, ITickable, IPausable, IPoolable<Vector3, IMemoryPool>
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
            TickableManager tickableManager)
        {
            _pauser = pauser;
            _tickableManager = tickableManager;
        }

        public void Start()
        {
            _expirationTime = _particleSystem.main.duration;
            _tickableManager.Add(this);
            _pauser.Register(this);
        }

        public void Pause()
        {
            if (_pool is not null)
                _particleSystem.Pause();
        }

        public void Resume()
        {
            if (_pool is not null)
                _particleSystem.Play();
        }

        public void OnDespawned()
        {
            _pool = null;
        }

        public void OnSpawned(
            Vector3 position,
            IMemoryPool pool)
        {
            transform.position = position;
            _lifeTime = 0f;
            _particleSystem.Clear();
            _particleSystem.Play();
            _pool = pool;
        }

        public void Tick()
        {
            if (_pool is not null)
            {
                _lifeTime += Time.deltaTime;
                if (_lifeTime > _expirationTime)
                    _pool.Despawn(this);
            }
        }

        public class Factory : PlaceholderFactory<Vector3, ParticleEffect>
        {
        }
    }
}