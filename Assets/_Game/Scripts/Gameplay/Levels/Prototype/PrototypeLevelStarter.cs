using Common.Loading;
using Gameplay.Levels.States;
using Zenject;

namespace Gameplay.Levels.Prototype
{
    public class PrototypeLevelStarter : IInitializable
    {
        private readonly AddressablesProvider _addressablesProvider;
        private readonly LevelStateMachine _levelStateMachine;


        public PrototypeLevelStarter(
            LevelStateMachine levelStateMachine,
            AddressablesProvider addressablesProvider)
        {
            _levelStateMachine = levelStateMachine;
            _addressablesProvider = addressablesProvider;
        }

        public void Initialize()
        {
            _addressablesProvider.ReleaseAssets();
            _levelStateMachine.Enter<LevelStartingState>();
        }
    }
}