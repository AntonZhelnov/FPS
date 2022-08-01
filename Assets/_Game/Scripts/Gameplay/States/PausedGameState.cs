using Common.Pausing;
using UI.Buttons;
using Zenject;

namespace Gameplay.States
{
    public class PausedGameState : GameState
    {
        private readonly Pauser _pauser;
        private readonly ResumeButton _resumeButton;
        private readonly TickableManager _tickableManager;


        public PausedGameState(
            Pauser pauser,
            ResumeButton resumeButton,
            TickableManager tickableManager)
        {
            _pauser = pauser;
            _resumeButton = resumeButton;
            _tickableManager = tickableManager;
        }

        public override void Initialize()
        {
            _resumeButton.Interacted += Resume;
        }

        public override void Dispose()
        {
            _resumeButton.Interacted -= Resume;
        }

        public override void Start()
        {
            _pauser.Pause();
            _tickableManager.IsPaused = true;
        }

        public override void Stop()
        {
            _pauser.Resume();
            _tickableManager.IsPaused = false;
        }

        public override void UpdateTime(float deltaTime)
        {
        }

        private void Resume()
        {
            StateMachine.SwitchState<PlayingGameState>();
        }

        public class Factory : PlaceholderFactory<PausedGameState>
        {
        }
    }
}