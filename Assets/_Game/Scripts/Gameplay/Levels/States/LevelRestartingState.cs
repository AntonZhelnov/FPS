using Common.States;
using Zenject;

namespace Gameplay.Levels.States
{
    public class LevelRestartingState : IState
    {
        private readonly LazyInject<LevelStateMachine> _levelStateMachine;
        private readonly SignalBus _signalBus;


        public LevelRestartingState(
            LazyInject<LevelStateMachine> levelStateMachine,
            SignalBus signalBus)
        {
            _levelStateMachine = levelStateMachine;
            _signalBus = signalBus;
        }

        public void Enter()
        {
            _signalBus.Fire<LevelRestartingSignal>();
        }

        public void Exit()
        {
        }

        public void Tick(float deltaTime)
        {
            _levelStateMachine.Value.Enter<LevelStartingState>();
        }
    }
}