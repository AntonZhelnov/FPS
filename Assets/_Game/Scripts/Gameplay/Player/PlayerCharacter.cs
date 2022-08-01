using Common;
using Configs;
using Gameplay.Weapons;
using UnityEngine;
using Zenject;

namespace Gameplay.Player
{
    public class PlayerCharacter : MonoBehaviour, IDamageable, IInitializable
    {
        [SerializeField] private Transform _weaponHolder;

        private Camera _camera;
        private Health.Factory _healthFactory;
        private PlayerCharacterConfig _playerCharacterConfig;
        private Vector3 _screenCenter;
        private Weapon _weapon;


        [Inject]
        public void Construct(
            Health.Factory healthFactory,
            PlayerCharacterConfig playerCharacterConfig,
            DamageGroup damageGroup)
        {
            DamageGroup = damageGroup;
            _healthFactory = healthFactory;
            _playerCharacterConfig = playerCharacterConfig;
        }

        public void Initialize()
        {
            Health = _healthFactory.Create(_playerCharacterConfig.PlayerCharacterHealthMax);

            _camera = Camera.main;
            _screenCenter = new Vector3(
                Screen.width / 2f,
                Screen.height / 2f,
                _camera.farClipPlane);
        }

        public Health Health { get; private set; }
        public DamageGroup DamageGroup { get; private set; }


        public void ReceiveDamage(int damage)
        {
            Health.TryDecrease(damage);
        }

        public void Attack()
        {
            _weapon.Attack(_camera.ScreenToWorldPoint(_screenCenter));
        }

        public void EquipWeapon(Weapon weapon)
        {
            _weapon = weapon;
            _weapon.SetParent(_weaponHolder);
            _weapon.transform.SetLayer(_weaponHolder.gameObject.layer);
        }
    }
}