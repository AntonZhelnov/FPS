using System;
using Configs;
using UnityEngine;
using Zenject;

namespace Common
{
    public class AnimationPlayer : IInitializable, ITickable
    {
        private readonly AnimationConfig _animationConfig;
        private readonly Animator _animator;
        private readonly Action _callback;
        private readonly TickableManager _tickableManager;
        private float _callbackDelay;
        private bool _isPlaying;
        private float _timePassed;
        private int _triggerHash;


        public AnimationPlayer(
            AnimationConfig animationConfig,
            Animator animator,
            Action callback,
            TickableManager tickableManager)
        {
            _animationConfig = animationConfig;
            _animator = animator;
            _callback = callback;
            _tickableManager = tickableManager;
        }

        [Inject]
        public void Initialize()
        {
            _tickableManager.Add(this);

            _triggerHash = _animationConfig.TriggerName.Length > 0
                ? Animator.StringToHash(_animationConfig.TriggerName)
                : 0;

            _callbackDelay = _callback != null
                ? _animationConfig.ActionDelay
                : 0;
        }

        public void Tick()
        {
            if (_isPlaying)
            {
                _timePassed += Time.deltaTime;
                if (_timePassed >= _callbackDelay)
                {
                    _isPlaying = false;
                    _callback?.Invoke();
                }
            }
        }

        public void Play()
        {
            if (!_isPlaying)
            {
                _animator.SetTrigger(_triggerHash);
                _timePassed = 0f;
                _isPlaying = true;
            }
        }

        public void Stop()
        {
            _animator.ResetTrigger(_triggerHash);
        }

        public class Factory : PlaceholderFactory<AnimationConfig, Animator, Action, AnimationPlayer>
        {
        }
    }
}