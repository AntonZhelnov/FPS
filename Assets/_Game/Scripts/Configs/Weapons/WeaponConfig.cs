using Gameplay.Weapons;
using UnityEngine;
using Zenject;

namespace Configs.Weapons
{
    public class WeaponConfig : ScriptableObjectInstaller
    {
        [SerializeField] private Weapon _weaponPrefab;
        [SerializeField] [Min(0.1f)] private float _attackDistance = 1f;
        [SerializeField] [Min(1)] private int _damage = 1;
        [SerializeField] [Min(0.1f)] private float _attacksPerSecond = 1f;

        public Weapon WeaponPrefab => _weaponPrefab;
        public float AttackDistance => _attackDistance;
        public int Damage => _damage;
        public float AttacksPerSecond => _attacksPerSecond;
    }
}