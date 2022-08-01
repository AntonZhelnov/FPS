using System.Collections.Generic;
using Common;
using Common.Pausing;
using Common.States;
using Gameplay.Player;
using Gameplay.States;
using Gameplay.Weapons;
using UI;
using Zenject;

namespace Starters
{
    public class TestLevelStarter : IInitializable
    {
        private readonly Gun.Factory _gunFactory;
        private readonly LostGameState.Factory _lostGameStateFactory;
        private readonly PausedGameState.Factory _pausedGameStateFactory;
        private readonly Pauser _pauser;
        private readonly PlayerCharacter _playerCharacter;
        private readonly PlayingGameState.Factory _playingGameStateFactory;
        private readonly StateMachine.Factory _stateMachineFactory;
        private readonly Ui _ui;


        public TestLevelStarter(
            StateMachine.Factory stateMachineFactory,
            PlayingGameState.Factory playingGameStateFactory,
            PausedGameState.Factory pausedGameStateFactory,
            LostGameState.Factory lostGameStateFactory,
            Pauser pauser,
            PlayerCharacter playerCharacter,
            Gun.Factory gunFactory,
            Ui ui)
        {
            _stateMachineFactory = stateMachineFactory;
            _playingGameStateFactory = playingGameStateFactory;
            _pausedGameStateFactory = pausedGameStateFactory;
            _lostGameStateFactory = lostGameStateFactory;
            _pauser = pauser;
            _playerCharacter = playerCharacter;
            _gunFactory = gunFactory;
            _ui = ui;
        }

        public void Initialize()
        {
            _pauser.Register(_ui);

            var gun = _gunFactory.Create(DamageGroup.Player);
            _playerCharacter.EquipWeapon(gun);

            var stateMachine = _stateMachineFactory.Create(
                new List<State>
                {
                    _playingGameStateFactory.Create(),
                    _pausedGameStateFactory.Create(),
                    _lostGameStateFactory.Create()
                }
            );

            stateMachine.Start();
        }
    }
}