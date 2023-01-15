using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Common.States
{
    public abstract class StateMachine : IDisposable, ITickable
    {
        private readonly TickableManager _tickableManager;
        private IState _currentState;

        protected Dictionary<Type, IState> States;


        protected StateMachine(TickableManager tickableManager)
        {
            _tickableManager = tickableManager;
        }

        public virtual void Dispose()
        {
            _tickableManager.Remove(this);
        }

        public void Tick()
        {
            _currentState?.Tick(Time.deltaTime);
        }

        public void Enter<T>() where T : IState
        {
            var nextState = States[typeof(T)];
            _currentState?.Exit();
            nextState.Enter();
            _currentState = nextState;
        }

        public void Exit()
        {
            _currentState?.Exit();
            _currentState = null;
        }
    }
}