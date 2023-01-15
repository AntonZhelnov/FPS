using System;
using Common.Saving;
using Cysharp.Threading.Tasks;
using UniRx;
using Zenject;

namespace Profile
{
    public class PlayerProfile : IInitializable, IDisposable
    {
        private readonly IDataProvider<PlayerProfileData> _playerProfileDataProvider;
        private readonly ReactiveProperty<int> _score;
        private readonly ReactiveProperty<int> _topScore;
        private PlayerProfileData _data;
        private IDisposable _scoreSubscription;


        public PlayerProfile(IDataProvider<PlayerProfileData> playerProfileDataProvider)
        {
            _playerProfileDataProvider = playerProfileDataProvider;

            _score = new ReactiveProperty<int>(0);
            Score = _score.ToReadOnlyReactiveProperty();

            _topScore = new ReactiveProperty<int>(0);
            TopScore = _topScore.ToReadOnlyReactiveProperty();
        }

        public void Initialize()
        {
            _scoreSubscription = _score
                .Where(value => value >= _topScore.Value)
                .Subscribe(UpdateTopScore);
        }

        public void Dispose()
        {
            _scoreSubscription.Dispose();
            _data.TopScore = _topScore.Value;
            Save();
        }

        public ReadOnlyReactiveProperty<int> Score { get; }
        public ReadOnlyReactiveProperty<int> TopScore { get; }


        public void IncreaseScore(int amount)
        {
            _score.Value += amount;
        }

        public async UniTask Load()
        {
            _data = await _playerProfileDataProvider.Load();
            _topScore.Value = _data.TopScore;
        }

        public void ResetScore()
        {
            _score.Value = 0;
        }

        public void Save()
        {
            _playerProfileDataProvider.Save();
        }

        private void UpdateTopScore(int value)
        {
            _topScore.Value = value;
        }
    }
}