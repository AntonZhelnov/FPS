using System;
using Common.Animating;
using Common.States;

namespace Gameplay.Enemy.States
{
    public class EnemyIdleState : IState
    {
        private readonly AnimationPlayer _animationPlayer;
        private readonly Settings _settings;


        public EnemyIdleState(
            Settings settings,
            AnimationPlayer animationPlayer)
        {
            _settings = settings;
            _animationPlayer = animationPlayer;
        }

        public void Enter()
        {
            _animationPlayer.Play(_settings.AnimationTriggerName);
        }

        public void Exit()
        {
        }

        public void Tick(float deltaTime)
        {
        }

        [Serializable]
        public class Settings
        {
            public string AnimationTriggerName;
        }
    }
}