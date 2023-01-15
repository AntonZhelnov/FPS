using System;
using System.Collections.Generic;
using Gameplay.Levels;
using Gameplay.Weapons;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay.Enemy
{
    public class EnemySpawner : IInitializable, IDisposable, ITickable
    {
        private readonly List<EnemySpawnPoint> _enemySpawnPoints = new();
        private readonly RandomWeaponProvider _randomWeaponProvider;
        private readonly Settings _settings;
        private readonly SignalBus _signalBus;
        private bool _isRunning;
        private int _spawnedEnemiesCount;
        private float _timePassed;


        public EnemySpawner(
            Settings settings,
            RandomWeaponProvider randomWeaponProvider,
            SignalBus signalBus)
        {
            _settings = settings;
            _randomWeaponProvider = randomWeaponProvider;
            _signalBus = signalBus;

            _timePassed = settings.SpawnInterval;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<LevelRestartingSignal>(ResetSpawnedEnemiesCount);
            _signalBus.Subscribe<EnemyDiedSignal>(DecreaseSpawnedEnemiesCount);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<LevelRestartingSignal>(ResetSpawnedEnemiesCount);
            _signalBus.Unsubscribe<EnemyDiedSignal>(DecreaseSpawnedEnemiesCount);
        }

        public void Tick()
        {
            if (_isRunning
                && _spawnedEnemiesCount < _settings.EnemiesMaximum)
            {
                _timePassed += Time.deltaTime;
                if (_timePassed >= _settings.SpawnInterval)
                {
                    Spawn();
                    _timePassed = 0f;
                }
            }
        }

        public void RegisterSpawnPoint(EnemySpawnPoint spawnPoint)
        {
            _enemySpawnPoints.Add(spawnPoint);
        }

        public void Run()
        {
            _timePassed = 0f;
            _isRunning = true;
        }

        public void Stop()
        {
            _isRunning = false;
        }

        private void DecreaseSpawnedEnemiesCount(EnemyDiedSignal _)
        {
            _spawnedEnemiesCount--;
        }

        private void IncreaseSpawnedEnemiesCount()
        {
            _spawnedEnemiesCount++;
        }

        private void ResetSpawnedEnemiesCount()
        {
            _spawnedEnemiesCount = 0;
        }

        private void Spawn()
        {
            var randomSpawnPoint = _enemySpawnPoints[Random.Range(0, _enemySpawnPoints.Count)];
            if (randomSpawnPoint.IsSafeToSpawn())
            {
                var enemy = randomSpawnPoint.Spawn();
                _randomWeaponProvider.GiveWeapon(enemy);
                IncreaseSpawnedEnemiesCount();
            }
            else
                _timePassed = _settings.SpawnInterval;
        }

        [Serializable]
        public class Settings
        {
            [Min(.1f)] public float SpawnInterval;
            [Min(1)] public int EnemiesMaximum;
        }
    }
}