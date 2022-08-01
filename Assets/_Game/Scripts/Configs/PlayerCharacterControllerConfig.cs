using UnityEngine;
using Zenject;

namespace Configs
{
    [CreateAssetMenu(
        fileName = "New Player Character Controller Config Installer",
        menuName = "Configs/Player/Player Character Controller Config Installer")]
    public class PlayerCharacterControllerConfig : ScriptableObjectInstaller
    {
        [SerializeField] [Min(0.1f)] private float _movementSpeed = 1f;
        [SerializeField] [Min(0.1f)] private float _lookSensitivity = 1f;
        [SerializeField] [Min(0.1f)] private float _cameraPitchLimit = 45f;

        public float MovementSpeed => _movementSpeed;
        public float LookSensitivity => _lookSensitivity;
        public float CameraPitchLimit => _cameraPitchLimit;


        public override void InstallBindings()
        {
            Container.BindInstance(this).AsSingle();
        }
    }
}