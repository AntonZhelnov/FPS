using System;
using UnityEngine;
using Zenject;

namespace Gameplay.Enemy
{
    public class EnemySpawnPoint : MonoBehaviour
    {
        private Camera _camera;
        private Enemy.Factory _factory;
        private Settings _settings;
        private EnemySpawner _spawner;


        [Inject]
        public void Construct(
            Settings settings,
            EnemySpawner spawner,
            Enemy.Factory factory,
            Camera camera)
        {
            _settings = settings;
            _spawner = spawner;
            _factory = factory;
            _camera = camera;
        }

        public void Start()
        {
            _spawner.RegisterSpawnPoint(this);
        }

        public bool IsSafeToSpawn()
        {
            var viewportPoint = _camera.WorldToViewportPoint(transform.position);

            var isVisibleByCamera = viewportPoint.x is > -.1f and < 1.1f
                                    && viewportPoint.y is > -.1f and < 1.1f
                                    && viewportPoint.z > -.1f;

            var isNearToCamera = Vector3.Distance(transform.position, _camera.transform.position)
                                 < _settings.SafeDistance;

            var isSafeToSpawn = !(isVisibleByCamera
                                  || isNearToCamera);

            return isSafeToSpawn;
        }

        public Enemy Spawn()
        {
            var enemy = _factory.Create();

            enemy.transform.SetPositionAndRotation(
                transform.position,
                transform.rotation);

            return enemy;
        }

        [Serializable]
        public class Settings
        {
            [Min(1f)] public float SafeDistance = 1f;
        }
    }
}