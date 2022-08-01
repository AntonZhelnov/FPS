using System;
using Common.States;

namespace Gameplay.States
{
    public class GameState : State, IDisposable
    {
        public virtual void Dispose()
        {
        }

        public override void Start()
        {
        }

        public override void Stop()
        {
        }

        public override void UpdateTime(float deltaTime)
        {
        }
    }
}