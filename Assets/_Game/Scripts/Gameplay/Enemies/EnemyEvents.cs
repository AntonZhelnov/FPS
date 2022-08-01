using System;

namespace Gameplay.Enemies
{
    public class EnemyEvents
    {
        public event Action<Enemy> Died;
        public event Action<Enemy> Spawned;


        public void OnDied(Enemy enemy)
        {
            Died?.Invoke(enemy);
        }

        public void OnSpawned(Enemy enemy)
        {
            Spawned?.Invoke(enemy);
        }
    }
}