using UnityEngine;

namespace Configs.Enemies
{
    [CreateAssetMenu(
        fileName = "New Enemy Spawn Area Config",
        menuName = "Configs/Enemies/Enemy Spawn Area Config")]
    public class EnemySpawnAreaConfig : ScriptableObject
    {
        [SerializeField] [Min(1f)] private float _minimumSpawnDistanceToPlayer = 1f;
        [SerializeField] [Min(0f)] private float _spawnableHeight;
        [SerializeField] private float _xMin, _xMax, _zMin, _zMax;

        public float MinimumSpawnDistanceToPlayer => _minimumSpawnDistanceToPlayer;
        public float SpawnableHeight => _spawnableHeight;
        public float XMin => _xMin;
        public float XMax => _xMax;
        public float ZMin => _zMin;
        public float ZMax => _zMax;
    }
}