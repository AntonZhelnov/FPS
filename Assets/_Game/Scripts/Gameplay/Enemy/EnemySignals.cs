namespace Gameplay.Enemy
{
    public struct EnemyDiedSignal
    {
        public EnemyDiedSignal(Enemy enemy)
        {
            Enemy = enemy;
        }

        public Enemy Enemy { get; }
    }
}