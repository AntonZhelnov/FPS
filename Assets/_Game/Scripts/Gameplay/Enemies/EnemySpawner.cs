using Common;
using Configs.Enemies;
using UnityEngine;
using Zenject;

namespace Gameplay.Enemies
{
    public class EnemySpawner : ITickable, IInitializable
    {
        private readonly EnemyEvents _enemyEvents;
        private readonly Enemy.Factory _enemyFactory;
        private readonly EnemySpawnConfig _enemySpawnConfig;
        private readonly IPositionGenerator _positionGenerator;
        private int _enemiesCountMax;
        private bool _isRunning;
        private int _spawnedEnemiesCount;
        private float _timeInterval;
        private float _timePassed;


        public EnemySpawner(
            Enemy.Factory enemyFactory,
            EnemySpawnConfig enemySpawnConfig,
            IPositionGenerator positionGenerator,
            EnemyEvents enemyEvents)
        {
            _enemyFactory = enemyFactory;
            _enemySpawnConfig = enemySpawnConfig;
            _positionGenerator = positionGenerator;
            _enemyEvents = enemyEvents;
        }

        public void Initialize()
        {
            _timeInterval = _enemySpawnConfig.SpawnInterval;
            _timePassed = _timeInterval;
            _enemiesCountMax = _enemySpawnConfig.EnemiesMaximum;
            _enemyEvents.Died += DecreaseSpawnedEnemiesCount;
            _isRunning = true;
        }

        public void Tick()
        {
            if (_isRunning
                && _spawnedEnemiesCount < _enemiesCountMax)
            {
                _timePassed += Time.deltaTime;
                if (_timePassed >= _timeInterval)
                {
                    Spawn();
                    _timePassed = 0f;
                }
            }
        }

        public void Stop()
        {
            _isRunning = false;
        }

        private void DecreaseSpawnedEnemiesCount(Enemy _)
        {
            _spawnedEnemiesCount--;
        }

        private void IncreaseSpawnedEnemiesCount()
        {
            _spawnedEnemiesCount++;
        }

        private void Spawn()
        {
            var enemy = _enemyFactory.Create();
            enemy.transform.position = _positionGenerator.Generate();

            IncreaseSpawnedEnemiesCount();
        }
    }
}