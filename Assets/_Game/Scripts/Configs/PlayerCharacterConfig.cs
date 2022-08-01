using UnityEngine;
using Zenject;

namespace Configs
{
    [CreateAssetMenu(
        fileName = "New Player Character Config Installer",
        menuName = "Configs/Player/Player Character Config Installer")]
    public class PlayerCharacterConfig : ScriptableObjectInstaller
    {
        [SerializeField] [Min(1)] private int _playerCharacterHealthMax = 100;

        public int PlayerCharacterHealthMax => _playerCharacterHealthMax;


        public override void InstallBindings()
        {
            Container.BindInstance(this).AsSingle();
        }
    }
}