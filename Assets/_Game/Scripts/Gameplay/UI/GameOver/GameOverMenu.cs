using Common.UI;
using Gameplay.Levels;
using Profile;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI.GameOver
{
    public class GameOverMenu : UIItem
    {
        [SerializeField] private TmpFormattedIntSetter _finalScoreText;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _exitButton;

        private PlayerProfile _playerProfile;
        private SignalBus _signalBus;


        [Inject]
        public void Construct(
            PlayerProfile playerProfile,
            SignalBus signalBus)
        {
            _playerProfile = playerProfile;
            _signalBus = signalBus;
        }

        public void Start()
        {
            _signalBus.Subscribe<LevelLostSignal>(Show);
            _signalBus.Subscribe<LevelRestartingSignal>(Hide);
            _restartButton.OnClickAsObservable()
                .Subscribe(_ => _signalBus.Fire<LevelRestartCommand>()).AddTo(this);
            _exitButton.OnClickAsObservable()
                .Subscribe(_ => _signalBus.Fire<LevelExitCommand>()).AddTo(this);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<LevelLostSignal>(Show);
            _signalBus.Unsubscribe<LevelRestartingSignal>(Hide);
        }

        protected override void Show()
        {
            _finalScoreText.SetValue(_playerProfile.Score.Value);
            base.Show();
        }
    }
}