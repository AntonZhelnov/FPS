using Input;
using UnityEngine;

namespace Gameplay.Player.Actions
{
    public class PlayerMove : IVector2Settable
    {
        private Vector2 _moveVector = Vector2.zero;


        public void SetVector2(Vector2 value)
        {
            _moveVector = value;
        }

        public Vector2 GetMove()
        {
            return _moveVector;
        }
    }
}