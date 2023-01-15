using System;
using System.Collections.Generic;
using Common.States;
using Zenject;

namespace Gameplay.Levels.States
{
    public class LevelStateMachine : StateMachine, IInitializable
    {
        private readonly SignalBus _signalBus;


        public LevelStateMachine(
            TickableManager tickableManager,
            LevelStartingState levelStartingState,
            LevelPlayingState levelPlayingState,
            LevelPausedState levelPausedState,
            LevelLostState levelLostState,
            LevelRestartingState levelRestartingState,
            SignalBus signalBus)
            : base(tickableManager)
        {
            States = new Dictionary<Type, IState>
            {
                { typeof(LevelStartingState), levelStartingState },
                { typeof(LevelPlayingState), levelPlayingState },
                { typeof(LevelPausedState), levelPausedState },
                { typeof(LevelLostState), levelLostState },
                { typeof(LevelRestartingState), levelRestartingState }
            };

            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<LevelRestartCommand>(Restart);
        }

        public override void Dispose()
        {
            _signalBus.Unsubscribe<LevelRestartCommand>(Restart);
            base.Dispose();
        }

        private void Restart()
        {
            Enter<LevelRestartingState>();
        }
    }
}