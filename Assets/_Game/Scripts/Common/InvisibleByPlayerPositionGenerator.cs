using Configs.Enemies;
using Gameplay.Player;
using UnityEngine;
using Zenject;

namespace Common
{
    public class InvisibleByPlayerPositionGenerator : IPositionGenerator, IInitializable
    {
        private readonly EnemySpawnAreaConfig _enemySpawnAreaConfig;
        private readonly PlayerCharacter _playerCharacter;
        private Camera _camera;
        private float _checkingHeight;
        private float _minimumDistanceToPlayer;
        private float _minimumDistanceToPlayerSquared;
        private Transform _playerTransform;
        private float _xMax;
        private float _xMin;
        private float _zMax;
        private float _zMin;


        public InvisibleByPlayerPositionGenerator(
            EnemySpawnAreaConfig enemySpawnAreaConfig,
            PlayerCharacter playerCharacter)
        {
            _enemySpawnAreaConfig = enemySpawnAreaConfig;
            _playerCharacter = playerCharacter;
        }

        public void Initialize()
        {
            _playerTransform = _playerCharacter.transform;

            _xMin = _enemySpawnAreaConfig.XMin;
            _xMax = _enemySpawnAreaConfig.XMax;
            _zMin = _enemySpawnAreaConfig.ZMin;
            _zMax = _enemySpawnAreaConfig.ZMax;

            _checkingHeight = _enemySpawnAreaConfig.SpawnableHeight;
            _minimumDistanceToPlayer = _enemySpawnAreaConfig.MinimumSpawnDistanceToPlayer;
            _minimumDistanceToPlayerSquared = Mathf.Pow(_minimumDistanceToPlayer, 2f);

            _camera = Camera.main;
        }

        public Vector3 Generate()
        {
            var newPosition = new Vector3(
                Random.Range(_xMin, _xMax),
                _checkingHeight,
                Random.Range(_zMin, _zMax));

            var viewportPoint = _camera.WorldToViewportPoint(newPosition);
            if (viewportPoint.x is >= -0.1f and <= 1.1f
                && viewportPoint.y is >= -0.1f and <= 1.1f
                && viewportPoint.z > -0.1f)
                newPosition = Generate();

            if ((newPosition - _playerTransform.position).sqrMagnitude < _minimumDistanceToPlayerSquared)
                newPosition += _minimumDistanceToPlayer * (newPosition - _playerTransform.position).normalized;

            newPosition.y = 0f;

            return newPosition;
        }
    }
}