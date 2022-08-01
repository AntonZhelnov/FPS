using Common;
using Common.States;
using Configs;

namespace Gameplay.Enemies.States
{
    public abstract class EnemyState : State
    {
        private readonly AnimationConfig _animationConfig;
        private readonly AnimationPlayer.Factory _animationPlayerFactory;
        protected readonly Enemy Enemy;
        protected AnimationPlayer AnimationPlayer;


        protected EnemyState(
            Enemy enemy,
            AnimationConfig animationConfig,
            AnimationPlayer.Factory animationPlayerFactory)
        {
            Enemy = enemy;
            _animationConfig = animationConfig;
            _animationPlayerFactory = animationPlayerFactory;
        }

        public override void Initialize()
        {
            base.Initialize();
            InitializeAnimationPlayer();
        }

        public override void Start()
        {
        }

        public override void Stop()
        {
        }

        public override void UpdateTime(float deltaTime)
        {
        }

        protected virtual void AnimationPlayerCallback()
        {
        }

        private void InitializeAnimationPlayer()
        {
            AnimationPlayer = _animationPlayerFactory.Create(
                _animationConfig,
                Enemy.Animator,
                AnimationPlayerCallback);
        }
    }
}