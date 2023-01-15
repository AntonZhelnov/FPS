namespace Gameplay.Player
{
    public struct PlayerDiedSignal
    {
    }

    public struct PlayerSpawnedSignal
    {
        public PlayerSpawnedSignal(Player player)
        {
            Player = player;
        }

        public Player Player { get; }
    }
}