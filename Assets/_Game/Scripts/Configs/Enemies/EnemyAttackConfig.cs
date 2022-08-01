using Configs.Weapons;
using Gameplay.Weapons;
using UnityEngine;

namespace Configs.Enemies
{
    [CreateAssetMenu(
        fileName = "New Enemy Attack Config",
        menuName = "Configs/Enemies/Enemy Attack Config")]
    public class EnemyAttackConfig : ScriptableObject
    {
        [SerializeField] private AnimationConfig _attackAnimationConfig;
        [SerializeField] private WeaponConfig _weaponConfig;

        public AnimationConfig AttackAnimationConfig => _attackAnimationConfig;
        public Weapon Weapon => _weaponConfig.WeaponPrefab;
    }
}