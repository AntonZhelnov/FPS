using System;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Gameplay.Player.Abilities
{
    public class PlayerAttacker : IInitializable, IDisposable, ITickable
    {
        private readonly Camera _camera;
        private readonly Controls _controls;
        private readonly PlayerWeaponHolder _weaponHolder;
        private bool _isAttacking;
        private Vector3 _screenCenter;


        public PlayerAttacker(
            Controls controls,
            PlayerWeaponHolder weaponHolder,
            Camera camera)
        {
            _controls = controls;
            _weaponHolder = weaponHolder;
            _camera = camera;
        }

        public void Initialize()
        {
            _screenCenter = new Vector3(
                Screen.width / 2f,
                Screen.height / 2f,
                _camera.farClipPlane);

            _controls.Player.Attack.started += StartAttack;
            _controls.Player.Attack.canceled += StopAttack;
        }

        public void Dispose()
        {
            _controls.Player.Attack.started -= StartAttack;
            _controls.Player.Attack.canceled -= StopAttack;
        }

        public void Tick()
        {
            if (_isAttacking)
                _weaponHolder.Use(_camera.ScreenToWorldPoint(_screenCenter));
        }

        private void StartAttack(InputAction.CallbackContext _)
        {
            _isAttacking = true;
        }

        private void StopAttack(InputAction.CallbackContext _)
        {
            _isAttacking = false;
        }
    }
}