using Input;
using UnityEngine;

namespace Gameplay.Player.Actions
{
    public class PlayerLook : IVector2Settable
    {
        private Vector2 _lookVector = Vector2.zero;


        public void SetVector2(Vector2 value)
        {
            _lookVector = value;
        }

        public Vector2 GetLook()
        {
            return _lookVector;
        }
    }
}