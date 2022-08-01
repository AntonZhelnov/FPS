using System;
using Zenject;

namespace Common
{
    public class Health : IInitializable
    {
        public event Action<int> Decreased;
        public event Action Depleted;

        private int _initialValue;
        private int _value;


        public Health(int value)
        {
            _value = value;
        }

        [Inject]
        public void Initialize()
        {
            _initialValue = _value;
        }

        public void Reset()
        {
            _value = _initialValue;
        }

        public bool TryDecrease(int amount)
        {
            if (_value > 0)
            {
                _value -= amount;
                Decreased?.Invoke(_value);

                if (_value <= 0)
                    Depleted?.Invoke();

                return true;
            }

            return false;
        }

        public class Factory : PlaceholderFactory<int, Health>
        {
        }
    }
}