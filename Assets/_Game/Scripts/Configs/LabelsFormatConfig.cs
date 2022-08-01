using UnityEngine;
using Zenject;

namespace Configs
{
    [CreateAssetMenu(
        fileName = "New Labels Format Config Installer",
        menuName = "Configs/Labels Format Config Installer")]
    public class LabelsFormatConfig : ScriptableObjectInstaller
    {
        [SerializeField] private string _playerHealthLabelFormat = "Health: {0}";
        [SerializeField] private string _playerScoreLabelFormat = "Score: {0}";

        public string PlayerHealthLabelFormat => _playerHealthLabelFormat;
        public string PlayerScoreLabelFormat => _playerScoreLabelFormat;


        public override void InstallBindings()
        {
            Container.BindInstance(this).AsSingle();
        }
    }
}