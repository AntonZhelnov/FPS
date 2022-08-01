using System;
using Zenject;

namespace Common
{
    public class Counter : IInitializable
    {
        public event Action<int> Changed;


        public void Initialize()
        {
            Reset();
        }

        public int Count { get; private set; }


        public void Increase(int amount)
        {
            Count += amount;
            Changed?.Invoke(Count);
        }

        private void Reset()
        {
            Count = 0;
            Changed?.Invoke(Count);
        }
    }
}