using Common.Pausing;
using Common.UI;
using Gameplay.Levels;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI
{
    public class PauseMenu : UIItem, IPausable
    {
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _exitButton;

        private Pauser _pauser;
        private SignalBus _signalBus;


        [Inject]
        public void Construct(
            Pauser pauser,
            SignalBus signalBus)
        {
            _signalBus = signalBus;
            _pauser = pauser;
        }

        public void Start()
        {
            _pauser.Register(this);

            _resumeButton.OnClickAsObservable()
                .Subscribe(_ => _signalBus.Fire<LevelResumeCommand>()).AddTo(this);

            _restartButton.OnClickAsObservable()
                .Subscribe(_ => _signalBus.Fire<LevelRestartCommand>()).AddTo(this);

            _exitButton.OnClickAsObservable()
                .Subscribe(_ => _signalBus.Fire<LevelExitCommand>()).AddTo(this);
        }

        public void Resume()
        {
            Hide();
        }

        public void Pause()
        {
            Show();
        }
    }
}