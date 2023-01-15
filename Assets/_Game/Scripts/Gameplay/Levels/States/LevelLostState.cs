using Common.States;
using Zenject;

namespace Gameplay.Levels.States
{
    public class LevelLostState : IState
    {
        private readonly SignalBus _signalBus;


        public LevelLostState(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void Enter()
        {
            _signalBus.Fire<LevelLostSignal>();
        }

        public void Exit()
        {
        }

        public void Tick(float deltaTime)
        {
        }
    }
}