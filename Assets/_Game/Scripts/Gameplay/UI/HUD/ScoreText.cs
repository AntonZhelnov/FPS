using Common.UI;
using Profile;
using UniRx;
using Zenject;

namespace Gameplay.UI.HUD
{
    public class ScoreText : TmpFormattedIntSetter
    {
        private ReadOnlyReactiveProperty<int> _score;


        [Inject]
        public void Construct(PlayerProfile playerProfile)
        {
            _score = playerProfile.Score;
        }

        public void Start()
        {
            _score.Subscribe(SetValue).AddTo(this);
        }
    }
}