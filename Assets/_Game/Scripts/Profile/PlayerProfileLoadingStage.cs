using System;
using Common.Loading;
using Cysharp.Threading.Tasks;

namespace Profile
{
    public class PlayerProfileLoadingStage : ILoadingStage
    {
        public event Action<float> Loaded;

        private readonly PlayerProfile _playerProfile;


        public PlayerProfileLoadingStage(PlayerProfile playerProfile)
        {
            _playerProfile = playerProfile;
        }

        public string Description => "Loading Player Profile...";


        public async UniTask Load()
        {
            Loaded?.Invoke(.5f);

            await _playerProfile.Load();

            Loaded?.Invoke(1f);
        }
    }
}