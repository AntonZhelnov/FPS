using System;
using Common.States;
using Gameplay.Enemy;
using Gameplay.Player;
using Input;
using Profile;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Gameplay.Levels.States
{
    public class LevelPlayingState : IState, IInitializable, IDisposable
    {
        private readonly ApplicationFocusReactor _applicationFocusReactor;
        private readonly CompositeDisposable _compositeDisposable = new();
        private readonly Controls _controls;
        private readonly EnemySpawner _enemySpawner;
        private readonly LazyInject<LevelStateMachine> _levelStateMachine;
        private readonly PlayerProfile _playerProfile;
        private readonly SignalBus _signalBus;


        public LevelPlayingState(
            PlayerProfile playerProfile,
            EnemySpawner enemySpawner,
            LazyInject<LevelStateMachine> levelStateMachine,
            Controls controls,
            ApplicationFocusReactor applicationFocusReactor,
            SignalBus signalBus)
        {
            _playerProfile = playerProfile;
            _enemySpawner = enemySpawner;
            _levelStateMachine = levelStateMachine;
            _controls = controls;
            _applicationFocusReactor = applicationFocusReactor;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.GetStream<LevelPauseCommand>()
                .Subscribe(_ => Pause())
                .AddTo(_compositeDisposable);

            _signalBus.GetStream<EnemyDiedSignal>()
                .Subscribe(signal => IncreaseScore(signal.Enemy.Score))
                .AddTo(_compositeDisposable);

            _signalBus.GetStream<PlayerDiedSignal>()
                .Subscribe(_ => Lose())
                .AddTo(_compositeDisposable);

            _controls.Player.Escape.performed += Pause;
            _applicationFocusReactor.ApplicationFocused += OnApplicationFocusChanged;
        }

        public void Dispose()
        {
            _compositeDisposable.Dispose();
            _controls.Player.Escape.performed -= Pause;
            _applicationFocusReactor.ApplicationFocused -= OnApplicationFocusChanged;
        }

        public void Enter()
        {
            _controls.Enable();
#if UNITY_EDITOR
            Cursor.lockState = CursorLockMode.Locked;
#endif
            _enemySpawner.Run();
        }

        public void Exit()
        {
            _controls.Disable();
#if UNITY_EDITOR
            Cursor.lockState = CursorLockMode.None;
#endif
            _enemySpawner.Stop();
        }

        public void Tick(float deltaTime)
        {
        }

        private void IncreaseScore(int value)
        {
            _playerProfile.IncreaseScore(value);
        }

        private void Lose()
        {
            _levelStateMachine.Value.Enter<LevelLostState>();
        }

        private void OnApplicationFocusChanged(bool isFocused)
        {
            if (!isFocused)
                Pause();
        }

        private void Pause(InputAction.CallbackContext _)
        {
            Pause();
        }

        private void Pause()
        {
            _levelStateMachine.Value.Enter<LevelPausedState>();
        }
    }
}