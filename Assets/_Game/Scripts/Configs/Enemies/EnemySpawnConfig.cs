using UnityEngine;
using Zenject;

namespace Configs.Enemies
{
    [CreateAssetMenu(
        fileName = "New Enemy Spawn Config Installer",
        menuName = "Configs/Enemies/Enemy Spawn Config Installer",
        order = -1)]
    public class EnemySpawnConfig : ScriptableObjectInstaller
    {
        [SerializeField] private EnemyConfig _enemyConfig;
        [SerializeField] private EnemySpawnAreaConfig _enemySpawnAreaConfig;
        [SerializeField] [Min(0.1f)] private float _spawnInterval = 3;
        [SerializeField] [Min(1)] private int _enemiesMaximum = 10;

        public EnemyConfig EnemyConfig => _enemyConfig;
        public EnemySpawnAreaConfig EnemySpawnAreaConfig => _enemySpawnAreaConfig;
        public float SpawnInterval => _spawnInterval;
        public int EnemiesMaximum => _enemiesMaximum;


        public override void InstallBindings()
        {
            Container.BindInstance(this).AsSingle();
        }
    }
}