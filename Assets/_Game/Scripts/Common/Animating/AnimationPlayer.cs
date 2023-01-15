using System;
using UnityEngine;
using Zenject;

namespace Common.Animating
{
    public class AnimationPlayer : ITickable
    {
        private readonly Animator _animator;
        private Action _callback;
        private float _callbackDelay;
        private bool _isWaiting;
        private float _timePassed;
        private int _triggerHash;
        private string _triggerName;


        public AnimationPlayer(Animator animator)
        {
            _animator = animator;
        }

        public void Tick()
        {
            if (_isWaiting)
            {
                _timePassed += Time.deltaTime;
                if (_timePassed >= _callbackDelay)
                {
                    _isWaiting = false;
                    _callback?.Invoke();
                }
            }
        }

        public void Play(
            string triggerName,
            float actionDelay = 0,
            Action callback = null)
        {
            if (_triggerName != triggerName)
            {
                _triggerName = triggerName;

                _triggerHash =
                    triggerName.Length > 0
                        ? Animator.StringToHash(triggerName)
                        : 0;
            }

            if (_callback != callback)
            {
                _callback = callback;

                _callbackDelay =
                    _callback is null
                        ? 0
                        : actionDelay;
            }

            _animator.SetTrigger(_triggerHash);
            _timePassed = 0f;
            _isWaiting = true;
        }

        public void Stop()
        {
            _isWaiting = false;
        }
    }
}