using System;
using UniRx;

namespace Common
{
    public class Health : IDisposable
    {
        public event Action Decreased;
        public event Action Depleted;

        private readonly ReactiveProperty<int> _health;
        private readonly int _initialValue;
        private readonly IDisposable _subscription;


        public Health(int value)
        {
            _health = new ReactiveProperty<int>(value);

            _subscription = _health
                .Where(amount => amount == 0)
                .Subscribe(_ => Depleted?.Invoke());

            Property = _health.ToReadOnlyReactiveProperty();

            _initialValue = value;
        }

        public void Dispose()
        {
            _subscription.Dispose();
        }

        public ReadOnlyReactiveProperty<int> Property { get; }


        public void Decrease(int amount)
        {
            if (_health.Value > 0)
            {
                _health.Value = Math.Max(0, _health.Value -= amount);
                Decreased?.Invoke();
            }
        }

        public void Increase(int amount)
        {
            _health.Value = Math.Min(_initialValue, _health.Value += amount);
        }

        public void Reset()
        {
            _health.Value = _initialValue;
        }
    }
}