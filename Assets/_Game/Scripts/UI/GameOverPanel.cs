using Zenject;

namespace UI
{
    public class GameOverPanel : UiElement
    {
        private PlayerFinalScoreText _playerFinalScoreText;


        [Inject]
        public void Construct(PlayerFinalScoreText playerFinalScoreText)
        {
            _playerFinalScoreText = playerFinalScoreText;
        }

        public void UpdateScore(int score)
        {
            _playerFinalScoreText.SetValue(score);
        }
    }
}