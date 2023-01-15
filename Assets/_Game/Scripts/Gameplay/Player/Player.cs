using System;
using Common;
using Gameplay.Damaging;
using Gameplay.Levels;
using Gameplay.Weapons;
using UnityEngine;
using Zenject;

namespace Gameplay.Player
{
    public class Player : MonoBehaviour, IDamageable, IWeaponUser
    {
        [SerializeField] private Transform _pitchTransform;
        [SerializeField] private Transform _cameraHolderTransform;

        private Camera _camera;
        private DamageReactor _damageReactor;
        private SignalBus _signalBus;
        private PlayerWeaponHolder _weaponHolder;


        [Inject]
        public void Construct(
            Health health,
            DamageGroup damageGroup,
            DamageReactor damageReactor,
            PlayerWeaponHolder playerWeaponHolder,
            Camera camera,
            SignalBus signalBus)
        {
            Health = health;
            DamageGroup = damageGroup;
            _damageReactor = damageReactor;
            _weaponHolder = playerWeaponHolder;
            _camera = camera;
            _signalBus = signalBus;
        }

        public Transform PitchTransform => _pitchTransform;
        public Health Health { get; private set; }


        public void Start()
        {
            AttachCamera();
            Health.Depleted += Die;
            _signalBus.Subscribe<LevelRestartingSignal>(Recycle);
            _signalBus.Fire(new PlayerSpawnedSignal(this));
        }

        public DamageGroup DamageGroup { get; private set; }


        public void ReceiveDamage(
            int damage,
            Vector3 position)
        {
            _damageReactor.React(
                damage,
                position);
        }

        public void EquipWeapon(Weapon weapon)
        {
            _weaponHolder.Equip(weapon);
        }

        private void AttachCamera()
        {
            _camera.transform.SetParent(_cameraHolderTransform);
            _camera.transform.localPosition = Vector3.zero;
            _camera.transform.localRotation = Quaternion.identity;
        }

        private void DetachCamera()
        {
            _camera.transform.SetParent(null);
        }

        private void Die()
        {
            Health.Depleted -= Die;
            _signalBus.Fire<PlayerDiedSignal>();
            DetachCamera();
        }

        private void Recycle()
        {
            _signalBus.Unsubscribe<LevelRestartingSignal>(Recycle);
            DetachCamera();
            _weaponHolder.RecycleWeapon();
            Destroy(gameObject);
        }

        public class Factory : PlaceholderFactory<Player>
        {
        }

        [Serializable]
        public class Settings
        {
            public Player Prefab;
            [Min(1)] public int Health = 100;
        }
    }
}