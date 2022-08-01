using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Common.States
{
    public class StateMachine : ITickable, IInitializable
    {
        private readonly List<State> _states;
        private readonly TickableManager _tickableManager;
        private State _currentState;
        private bool _isRunning;


        public StateMachine(
            List<State> states,
            TickableManager tickableManager)
        {
            _states = states;
            _tickableManager = tickableManager;
        }

        [Inject]
        public void Initialize()
        {
            foreach (var state in _states)
                state.StateMachine = this;
        }

        public void Tick()
        {
            _currentState?.UpdateTime(Time.deltaTime);
        }

        public void Start()
        {
            if (!_isRunning)
            {
                _currentState = _states[0];
                _currentState.Start();
                _tickableManager.Add(this);
                _isRunning = true;
            }
        }

        public void Stop()
        {
            if (_isRunning)
            {
                _tickableManager.Remove(this);
                _isRunning = false;
            }
        }

        public void SwitchState<T>() where T : State
        {
            var state = _states.FirstOrDefault(s => s is T);
            if (state == null)
                throw new InvalidOperationException($"State \"{typeof(T).Name}\" not found!");
            _currentState.Stop();
            state.Start();
            _currentState = state;
        }

        public class Factory : PlaceholderFactory<List<State>, StateMachine>
        {
        }
    }
}