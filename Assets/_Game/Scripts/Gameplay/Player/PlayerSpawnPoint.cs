using UnityEngine;
using Zenject;

namespace Gameplay.Player
{
    public class PlayerSpawnPoint : MonoBehaviour
    {
        private Player.Factory _playerFactory;


        [Inject]
        public void Construct(Player.Factory playerFactory)
        {
            _playerFactory = playerFactory;
        }

        public Player Spawn()
        {
            var player = _playerFactory.Create();

            player.transform.SetPositionAndRotation(
                transform.position,
                transform.rotation);

            return player;
        }
    }
}