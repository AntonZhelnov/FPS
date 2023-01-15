using Common.UI;
using Profile;
using UniRx;
using Zenject;

namespace Gameplay.UI.HUD
{
    public class TopScoreText : TmpFormattedIntSetter
    {
        private ReadOnlyReactiveProperty<int> _topScore;


        [Inject]
        public void Construct(PlayerProfile playerProfile)
        {
            _topScore = playerProfile.TopScore;
        }

        public void Start()
        {
            _topScore.Subscribe(SetValue).AddTo(this);
        }
    }
}