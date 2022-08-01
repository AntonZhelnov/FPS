using Common;
using Gameplay.Enemies;
using UI;
using UI.Buttons;
using Zenject;

namespace Gameplay.States
{
    public class LostGameState : GameState
    {
        private readonly EnemySpawner _enemySpawner;
        private readonly RestartButton _restartButton;
        private readonly SceneReloader _sceneReloader;
        private readonly Ui _ui;


        public LostGameState(
            EnemySpawner enemySpawner,
            Ui ui,
            SceneReloader sceneReloader,
            RestartButton restartButton)
        {
            _enemySpawner = enemySpawner;
            _ui = ui;
            _sceneReloader = sceneReloader;
            _restartButton = restartButton;
        }

        public override void Initialize()
        {
            _restartButton.Interacted += Restart;
        }

        public override void Dispose()
        {
            _restartButton.Interacted -= Restart;
        }

        public override void Start()
        {
            _enemySpawner.Stop();
            _ui.ShowGameOver();
        }

        public override void Stop()
        {
        }

        public override void UpdateTime(float deltaTime)
        {
        }

        private void Restart()
        {
            _sceneReloader.Reload();
        }

        public class Factory : PlaceholderFactory<LostGameState>
        {
        }
    }
}