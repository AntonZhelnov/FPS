using UnityEngine;
using Zenject;

namespace Common.Pausing
{
    public class AnimatorPauser : IInitializable, IPausable
    {
        private readonly Animator _animator;
        private readonly Pauser _pauser;
        private float _animatorSpeed;


        public AnimatorPauser(
            Animator animator,
            Pauser pauser)
        {
            _animator = animator;
            _pauser = pauser;
        }

        public void Initialize()
        {
            _pauser.Register(this);
        }

        public void Pause()
        {
            _animatorSpeed = _animator.speed;
            _animator.speed = 0f;
        }

        public void Resume()
        {
            _animator.speed = _animatorSpeed;
        }
    }
}