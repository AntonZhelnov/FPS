using Common.UI;
using Gameplay.Player;
using UniRx;
using Zenject;

namespace Gameplay.UI.HUD
{
    public class PlayerHealthText : TmpFormattedIntSetter
    {
        private SignalBus _signalBus;


        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void Start()
        {
            _signalBus.Subscribe<PlayerSpawnedSignal>(SubscribeToHealth);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<PlayerSpawnedSignal>(SubscribeToHealth);
        }

        private void SubscribeToHealth(PlayerSpawnedSignal signal)
        {
            signal.Player.Health.Property
                .Subscribe(SetValue).AddTo(this);
        }
    }
}