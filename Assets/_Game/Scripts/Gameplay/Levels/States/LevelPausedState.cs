using System;
using Common.Pausing;
using Common.States;
using Zenject;

namespace Gameplay.Levels.States
{
    public class LevelPausedState : IState, IInitializable, IDisposable
    {
        private readonly LazyInject<LevelStateMachine> _levelStateMachine;
        private readonly Pauser _pauser;
        private readonly SignalBus _signalBus;


        public LevelPausedState(
            Pauser pauser,
            LazyInject<LevelStateMachine> levelStateMachine,
            SignalBus signalBus)
        {
            _pauser = pauser;
            _levelStateMachine = levelStateMachine;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<LevelResumeCommand>(Resume);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<LevelResumeCommand>(Resume);
        }

        public void Enter()
        {
            _pauser.Pause();
        }

        public void Exit()
        {
            _pauser.Resume();
        }

        public void Tick(float deltaTime)
        {
        }

        private void Resume()
        {
            _levelStateMachine.Value.Enter<LevelPlayingState>();
        }
    }
}