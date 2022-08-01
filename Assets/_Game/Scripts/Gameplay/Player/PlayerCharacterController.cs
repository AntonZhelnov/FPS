using Configs;
using Gameplay.Player.Actions;
using UnityEngine;
using Zenject;

namespace Gameplay.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerCharacterController : MonoBehaviour, ITickable, IInitializable
    {
        [SerializeField] private Transform _pitchTransform;

        private float _cameraPitchLimit;
        private CharacterController _characterController;
        private float _currentHeadPitchRotation;
        private float _lookSensitivity;
        private float _movementSpeed;
        private PlayerActions _playerActions;


        [Inject]
        public void Construct(
            PlayerCharacterControllerConfig playerCharacterControllerConfig,
            PlayerActions playerActions)
        {
            _movementSpeed = playerCharacterControllerConfig.MovementSpeed;
            _lookSensitivity = playerCharacterControllerConfig.LookSensitivity;
            _cameraPitchLimit = playerCharacterControllerConfig.CameraPitchLimit;
            _playerActions = playerActions;
        }

        public void Initialize()
        {
            _characterController = GetComponent<CharacterController>();
        }

        public void Tick()
        {
            UpdatePosition();
            UpdateRotation();
        }

        private void UpdatePosition()
        {
            var movementInput = _playerActions.GetMove();
            var movement = new Vector3(movementInput.x, 0f, movementInput.y);
            movement = movement.z * _pitchTransform.forward + movement.x * _pitchTransform.right;
            movement.y = 0f;

            _characterController.Move(Time.deltaTime * _movementSpeed * movement);
        }

        private void UpdateRotation()
        {
            var lookXInput = _playerActions.GetLook().x;
            var yawRotation = _lookSensitivity * new Vector3(0f, lookXInput, 0f);
            transform.Rotate(yawRotation);

            var lookYInput = _playerActions.GetLook().y;
            var pitchRotation = lookYInput * _lookSensitivity;
            _currentHeadPitchRotation -= pitchRotation;
            _currentHeadPitchRotation = Mathf.Clamp(_currentHeadPitchRotation, -_cameraPitchLimit, _cameraPitchLimit);
            _pitchTransform.localEulerAngles = new Vector3(_currentHeadPitchRotation, 0f, 0f);
        }
    }
}