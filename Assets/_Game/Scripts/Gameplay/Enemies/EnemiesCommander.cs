using System.Collections.Generic;

namespace Gameplay.Enemies
{
    public class EnemiesCommander
    {
        private readonly List<Enemy> _enemies = new();


        public void CommandToStayStill()
        {
            for (var i = 0; i < _enemies.Count; i++)
                _enemies[i].StayStill();
        }

        public void RegisterEnemy(Enemy enemy)
        {
            _enemies.Add(enemy);
        }

        public void UnregisterEnemy(Enemy enemy)
        {
            _enemies.Remove(enemy);
        }
    }
}