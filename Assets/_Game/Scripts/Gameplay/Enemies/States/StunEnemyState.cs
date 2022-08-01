using Common;
using Configs;
using Zenject;

namespace Gameplay.Enemies.States
{
    public class StunEnemyState : EnemyState
    {
        public StunEnemyState(
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

        protected override void AnimationPlayerCallback()
        {
            Continue();
        }

        private void Continue()
        {
            if (Enemy.IsWithinAttackRange())
                StateMachine.SwitchState<AttackingPlayerEnemyState>();
            else
                StateMachine.SwitchState<ChasingPlayerEnemyState>();
        }

        public class Factory : PlaceholderFactory<Enemy, AnimationConfig, StunEnemyState>
        {
        }
    }
}