using Common.Pausing;
using Common.UI;
using Gameplay.Levels;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI
{
    public class ScreenControls : UIItem, IPausable
    {
        [SerializeField] private Button _pauseButton;

        private Pauser _pauser;
        private SignalBus _signalBus;


        [Inject]
        public void Construct(
            Pauser pauser,
            SignalBus signalBus)
        {
            _pauser = pauser;
            _signalBus = signalBus;
        }

        public void Start()
        {
            _pauseButton.OnClickAsObservable()
                .Subscribe(_ => _signalBus.Fire<LevelPauseCommand>()).AddTo(this);

            _pauser.Register(this);
            _signalBus.Subscribe<LevelLostSignal>(Hide);
            _signalBus.Subscribe<LevelStartedSignal>(Show);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<LevelLostSignal>(Hide);
            _signalBus.Unsubscribe<LevelStartedSignal>(Show);
        }

        public void Pause()
        {
            Hide();
        }

        public void Resume()
        {
            Show();
        }
    }
}