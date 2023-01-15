using System;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Gameplay.Player.Abilities
{
    public class PlayerLooker : IInitializable, IDisposable
    {
        private readonly Controls _controls;
        private readonly Transform _pitchTransform;
        private readonly PlayerControlSettings _playerControlSettings;
        private readonly Transform _playerTransform;
        private float _currentHeadPitchRotation;
        private Vector2 _currentPosition;
        private Vector2 _delta;
        private Vector2 _previousPosition;


        public PlayerLooker(
            Controls controls,
            Player player,
            PlayerControlSettings playerControlSettings)
        {
            _controls = controls;
            _playerControlSettings = playerControlSettings;

            _playerTransform = player.transform;
            _pitchTransform = player.PitchTransform;
        }

        public void Initialize()
        {
            _controls.Player.Look.performed += SetDirection;
            _controls.Player.Look.canceled += ResetDirection;
        }

        public void Dispose()
        {
            _controls.Player.Look.performed -= SetDirection;
            _controls.Player.Look.canceled -= ResetDirection;
        }

        private void ResetDirection(InputAction.CallbackContext callbackContext)
        {
            _currentPosition = Vector2.zero;
            _previousPosition = Vector2.zero;
            _delta = Vector2.zero;
            UpdateLook();
        }

        private void SetDirection(InputAction.CallbackContext callbackContext)
        {
            _currentPosition = callbackContext.ReadValue<Vector2>();
            _delta = _currentPosition - _previousPosition;
            _previousPosition = _currentPosition;
            UpdateLook();
        }

        private void UpdateLook()
        {
            var lookSensitivity = _playerControlSettings.LookSensitivity;
            var cameraPitchLimit = _playerControlSettings.CameraPitchLimit;
            var yawRotation = lookSensitivity * new Vector3(0f, _delta.x, 0f);
            _playerTransform.Rotate(yawRotation);

            var pitchRotation = _delta.y * lookSensitivity;
            _currentHeadPitchRotation -= pitchRotation;
            _currentHeadPitchRotation = Mathf.Clamp(_currentHeadPitchRotation, -cameraPitchLimit, cameraPitchLimit);
            _pitchTransform.localEulerAngles = new Vector3(_currentHeadPitchRotation, 0f, 0f);
        }
    }
}