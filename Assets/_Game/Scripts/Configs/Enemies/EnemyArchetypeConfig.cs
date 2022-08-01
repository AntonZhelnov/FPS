using UnityEngine;

namespace Configs.Enemies
{
    [CreateAssetMenu(
        fileName = "New Enemy Archetype Config",
        menuName = "Configs/Enemies/Enemy Archetype Config")]
    public class EnemyArchetypeConfig : ScriptableObject
    {
        [SerializeField] [Min(1)] private int _health = 1;
        [SerializeField] [Min(0.1f)] private float _attacksPerSecond = 1;
        [SerializeField] [Min(1)] private int _score = 1;

        public int Health => _health;
        public float AttacksPerSecond => _attacksPerSecond;
        public int Score => _score;
    }
}