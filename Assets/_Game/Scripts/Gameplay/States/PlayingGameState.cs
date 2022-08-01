using Common;
using Common.Effects;
using Gameplay.Enemies;
using Gameplay.Player;
using Gameplay.Player.Actions;
using Gameplay.Weapons.Shooting;
using UI;
using UI.Buttons;
using UnityEngine;
using Zenject;

namespace Gameplay.States
{
    public class PlayingGameState : GameState
    {
        private readonly BulletEvents _bulletEvents;
        private readonly EnemiesCommander _enemiesCommander;
        private readonly EnemyEvents _enemyEvents;
        private readonly EffectSpawner _hitEffectSpawner;
        private readonly PauseButton _pauseButton;
        private readonly PlayerActions _playerActions;
        private readonly PlayerCharacter _playerCharacter;
        private readonly Counter _scoreCounter;
        private readonly Ui _ui;
        private Transform _playerCharacterTransform;
        private Health _playerHealth;


        public PlayingGameState(
            PlayerCharacter playerCharacter,
            PlayerActions playerActions,
            EnemyEvents enemyEvents,
            BulletEvents bulletEvents,
            Counter scoreCounter,
            EffectSpawner hitEffectSpawner,
            EnemiesCommander enemiesCommander,
            PauseButton pauseButton,
            Ui ui)
        {
            _playerCharacter = playerCharacter;
            _playerActions = playerActions;
            _enemyEvents = enemyEvents;
            _bulletEvents = bulletEvents;
            _scoreCounter = scoreCounter;
            _hitEffectSpawner = hitEffectSpawner;
            _enemiesCommander = enemiesCommander;
            _pauseButton = pauseButton;
            _ui = ui;
        }

        public override void Initialize()
        {
            _playerHealth = _playerCharacter.Health;
            _playerCharacterTransform = _playerCharacter.transform;

            _pauseButton.Interacted += Pause;
            _enemyEvents.Spawned += _enemiesCommander.RegisterEnemy;
            _enemyEvents.Died += _enemiesCommander.UnregisterEnemy;
            _enemyEvents.Died += IncreaseScore;
            _scoreCounter.Changed += ReactOnScoreChange;
            _playerHealth.Decreased += ReactOnPlayerDamage;
            _playerHealth.Depleted += Lose;
            _bulletEvents.Hitted += ReactOnBulletHit;
        }

        public override void Dispose()
        {
            _pauseButton.Interacted -= Pause;
            _enemyEvents.Spawned -= _enemiesCommander.RegisterEnemy;
            _enemyEvents.Died -= _enemiesCommander.UnregisterEnemy;
            _enemyEvents.Died -= IncreaseScore;
            _scoreCounter.Changed -= ReactOnScoreChange;
            _playerHealth.Decreased -= ReactOnPlayerDamage;
            _playerHealth.Depleted -= Lose;
            _bulletEvents.Hitted -= ReactOnBulletHit;
        }

        public override void UpdateTime(float deltaTime)
        {
            if (_playerActions.GetAttack())
                _playerCharacter.Attack();
        }

        private void IncreaseScore(Enemy enemy)
        {
            _scoreCounter.Increase(enemy.Score);
        }

        private void Lose()
        {
            _enemiesCommander.CommandToStayStill();
            StateMachine.SwitchState<LostGameState>();
        }

        private void Pause()
        {
            StateMachine.SwitchState<PausedGameState>();
        }

        private void ReactOnBulletHit(Bullet bullet)
        {
            SpawnHitEffect(bullet.transform.position);
        }

        private void ReactOnPlayerDamage(int newHealthValue)
        {
            SpawnHitEffect(_playerCharacterTransform.position);
            _ui.UpdatePlayerHealth(newHealthValue);
        }

        private void ReactOnScoreChange(int newScoreValue)
        {
            _ui.UpdateScore(newScoreValue);
        }

        private void SpawnHitEffect(Vector3 spawnPosition)
        {
            _hitEffectSpawner.Spawn(spawnPosition);
        }

        public class Factory : PlaceholderFactory<PlayingGameState>
        {
        }
    }
}