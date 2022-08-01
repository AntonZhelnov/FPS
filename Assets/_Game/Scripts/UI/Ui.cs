using Common;
using Common.Pausing;
using UI.Buttons;
using Zenject;

namespace UI
{
    public class Ui : IPausable
    {
        private GameOverPanel _gameOverPanel;
        private HUD _hud;
        private Inputs _inputs;
        private PauseButton _pauseButton;
        private ResumeButton _resumeButton;
        private Counter _scoreCounter;


        [Inject]
        public void Construct(
            HUD hud,
            Inputs inputs,
            PauseButton pauseButton,
            ResumeButton resumeButton,
            GameOverPanel gameOverPanel,
            Counter scoreCounter)
        {
            _hud = hud;
            _inputs = inputs;
            _pauseButton = pauseButton;
            _resumeButton = resumeButton;
            _gameOverPanel = gameOverPanel;
            _scoreCounter = scoreCounter;
        }

        public void Resume()
        {
            _pauseButton.Show();
            _hud.Show();
            _inputs.Show();
            _resumeButton.Hide();
        }

        public void Pause()
        {
            _pauseButton.Hide();
            _hud.Hide();
            _inputs.Hide();
            _resumeButton.Show();
        }

        public void ShowGameOver()
        {
            _pauseButton.Hide();
            _hud.Hide();
            _inputs.Hide();
            _resumeButton.Hide();

            _gameOverPanel.UpdateScore(_scoreCounter.Count);
            _gameOverPanel.Show();
        }

        public void UpdatePlayerHealth(int newHealthValue)
        {
            _hud.UpdatePlayerHealth(newHealthValue);
        }

        public void UpdateScore(int newScoreValue)
        {
            _hud.UpdateScore(newScoreValue);
        }
    }
}