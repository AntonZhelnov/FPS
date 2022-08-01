using Zenject;

namespace UI
{
    public class HUD : UiElement
    {
        private PlayerCharacterHealthText _playerCharacterHealthText;
        private PlayerScoreText _playerScoreText;


        [Inject]
        public void Construct(
            PlayerCharacterHealthText playerCharacterHealthText,
            PlayerScoreText playerScoreText)
        {
            _playerCharacterHealthText = playerCharacterHealthText;
            _playerScoreText = playerScoreText;
        }

        public void UpdatePlayerHealth(int newHealthValue)
        {
            _playerCharacterHealthText.SetValue(newHealthValue);
        }

        public void UpdateScore(int newScoreValue)
        {
            _playerScoreText.SetValue(newScoreValue);
        }
    }
}