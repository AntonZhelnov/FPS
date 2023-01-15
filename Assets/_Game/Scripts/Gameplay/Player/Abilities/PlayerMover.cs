using System;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Gameplay.Player.Abilities
{
    public class PlayerMover : ITickable, IInitializable, IDisposable
    {
        private readonly CharacterController _characterController;
        private readonly Controls _controls;
        private readonly Transform _pitchTransform;
        private readonly PlayerControlSettings _playerControlSettings;
        private Vector2 _direction;
        private bool _isMoving;


        public PlayerMover(
            Controls controls,
            PlayerControlSettings playerControlSettings,
            CharacterController characterController,
            Player player)
        {
            _controls = controls;
            _characterController = characterController;
            _playerControlSettings = playerControlSettings;

            _pitchTransform = player.PitchTransform;
        }

        public void Initialize()
        {
            _controls.Player.Move.performed += SetDirection;
            _controls.Player.Move.canceled += ResetDirection;
        }

        public void Dispose()
        {
            _controls.Player.Move.performed -= SetDirection;
            _controls.Player.Move.canceled -= ResetDirection;
        }

        public void Tick()
        {
            if (_isMoving)
                UpdateMovement();
        }

        private void ResetDirection(InputAction.CallbackContext callbackContext)
        {
            _direction = Vector2.zero;
            _isMoving = false;
        }

        private void SetDirection(InputAction.CallbackContext callbackContext)
        {
            _direction = callbackContext.ReadValue<Vector2>();
            _isMoving = true;
        }

        private void UpdateMovement()
        {
            var direction = new Vector3(_direction.x, 0f, _direction.y);
            direction = direction.z * _pitchTransform.forward + direction.x * _pitchTransform.right;
            direction.y = 0f;

            _characterController.Move(Time.deltaTime * _playerControlSettings.MovementSpeed * direction);
        }
    }
}