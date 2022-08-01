using Common;
using Configs;
using Zenject;

namespace Gameplay.Enemies.States
{
    public class IdleEnemyState : EnemyState
    {
        public IdleEnemyState(
            Enemy enemy,
            AnimationConfig animationConfig,
            AnimationPlayer.Factory animationPlayerFactory)
            : base(enemy, animationConfig, animationPlayerFactory)
        {
        }

        public override void Start()
        {
            AnimationPlayer.Play();
        }

        public class Factory : PlaceholderFactory<Enemy, AnimationConfig, IdleEnemyState>
        {
        }
    }
}