using Common;
using Configs;
using Zenject;

namespace Gameplay.Enemies.States
{
    public class HurtEnemyState : EnemyState
    {
        public HurtEnemyState(
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

        public override void UpdateTime(float deltaTime)
        {
            Continue();
        }

        private void Continue()
        {
            if (Enemy.IsWithinAttackRange())
                StateMachine.SwitchState<StunEnemyState>();
            else
                StateMachine.SwitchState<ChasingPlayerEnemyState>();
        }

        public class Factory : PlaceholderFactory<Enemy, AnimationConfig, HurtEnemyState>
        {
        }
    }
}