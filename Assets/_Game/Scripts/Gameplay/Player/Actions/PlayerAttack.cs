using Input;

namespace Gameplay.Player.Actions
{
    public class PlayerAttack : IBoolSettable
    {
        private bool _attackState;


        public void SetBool(bool value)
        {
            _attackState = value;
        }

        public bool GetAttack()
        {
            return _attackState;
        }
    }
}