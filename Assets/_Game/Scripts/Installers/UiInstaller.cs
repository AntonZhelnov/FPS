using Configs;
using UI;
using Zenject;

namespace Installers
{
    public class UiInstaller : MonoInstaller
    {
        [Inject] private LabelsFormatConfig _labelsFormatConfig;
        [Inject] private PlayerCharacterConfig _playerCharacterConfig;


        public override void InstallBindings()
        {
            Container.Bind<Ui>().AsSingle();

            Container.BindInstance(_labelsFormatConfig.PlayerHealthLabelFormat)
                .WhenInjectedInto<PlayerCharacterHealthText>();
            Container.BindInstance(_playerCharacterConfig.PlayerCharacterHealthMax)
                .WhenInjectedInto<PlayerCharacterHealthText>();

            Container.BindInstance(_labelsFormatConfig.PlayerScoreLabelFormat)
                .WhenInjectedInto<PlayerScoreText>();
            Container.BindInstance(0)
                .WhenInjectedInto<PlayerScoreText>();

            Container.BindInstance(_labelsFormatConfig.PlayerScoreLabelFormat)
                .WhenInjectedInto<PlayerFinalScoreText>();
            Container.BindInstance(0)
                .WhenInjectedInto<PlayerFinalScoreText>();
        }
    }
}