using Common.States;
using Gameplay.Enemy;
using Gameplay.Player;
using Gameplay.Weapons.Gun;
using Profile;
using Zenject;

namespace Gameplay.Levels.States
{
    public class LevelStartingState : IState
    {
        private readonly EnemyTarget _enemyTarget;
        private readonly Gun.Factory _gunFactory;
        private readonly LazyInject<LevelStateMachine> _levelStateMachine;
        private readonly PlayerProfile _playerProfile;
        private readonly PlayerSpawnPoint _playerSpawnPoint;
        private readonly SignalBus _signalBus;


        public LevelStartingState(
            PlayerProfile playerProfile,
            PlayerSpawnPoint playerSpawnPoint,
            Gun.Factory gunFactory,
            EnemyTarget enemyTarget,
            LazyInject<LevelStateMachine> levelStateMachine,
            SignalBus signalBus)
        {
            _playerProfile = playerProfile;
            _playerSpawnPoint = playerSpawnPoint;
            _gunFactory = gunFactory;
            _enemyTarget = enemyTarget;
            _levelStateMachine = levelStateMachine;
            _signalBus = signalBus;
        }

        public void Enter()
        {
            _playerProfile.ResetScore();

            var player = _playerSpawnPoint.Spawn();
            var gun = _gunFactory.Create();
            player.EquipWeapon(gun);

            _enemyTarget.Transform = player.transform;
            _signalBus.Fire<LevelStartedSignal>();
        }

        public void Exit()
        {
        }

        public void Tick(float deltaTime)
        {
            _levelStateMachine.Value.Enter<LevelPlayingState>();
        }
    }
}