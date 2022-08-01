using UnityEngine;

namespace Gameplay.Player.Actions
{
    public class PlayerActions
    {
        public PlayerActions()
        {
            PlayerMove = new PlayerMove();
            PlayerLook = new PlayerLook();
            PlayerAttack = new PlayerAttack();
        }

        public PlayerMove PlayerMove { get; }
        public PlayerLook PlayerLook { get; }
        public PlayerAttack PlayerAttack { get; }


        public bool GetAttack()
        {
            return PlayerAttack.GetAttack();
        }

        public Vector2 GetLook()
        {
            return PlayerLook.GetLook();
        }

        public Vector2 GetMove()
        {
            return PlayerMove.GetMove();
        }
    }
}