using System.Collections.Generic;
using Gameplay.Enemies;
using UnityEngine;

namespace Configs.Enemies
{
    [CreateAssetMenu(
        fileName = "New Enemy Config",
        menuName = "Configs/Enemies/Enemy Config")]
    public class EnemyConfig : ScriptableObject
    {
        [SerializeField] private Enemy _enemyPrefab;
        [SerializeField] private AnimationConfig _moveAnimationConfig;
        [SerializeField] private AnimationConfig _hurtAnimationConfig;
        [SerializeField] private AnimationConfig _idleAnimationConfig;
        [SerializeField] private AnimationConfig _deadAnimationConfig;
        [SerializeField] private List<EnemyArchetypeConfig> _enemyArchetypeConfigs;
        [SerializeField] private List<EnemyAttackConfig> _enemyAttackConfigs;

        public Enemy EnemyPrefab => _enemyPrefab;
        public AnimationConfig MoveAnimationConfig => _moveAnimationConfig;
        public AnimationConfig HurtAnimationConfig => _hurtAnimationConfig;
        public AnimationConfig IdleAnimationConfig => _idleAnimationConfig;
        public AnimationConfig DeadAnimationConfig => _deadAnimationConfig;
        public List<EnemyArchetypeConfig> EnemyArchetypeConfigs => _enemyArchetypeConfigs;
        public List<EnemyAttackConfig> EnemyAttackConfigs => _enemyAttackConfigs;
    }
}