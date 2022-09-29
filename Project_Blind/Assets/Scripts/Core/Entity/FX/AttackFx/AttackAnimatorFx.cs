using UnityEngine;

namespace Blind 
{
    [RequireComponent(typeof(Animator))]
    public class AttackAnimatorFx : AttackFX
    {
        private Animator _animator;

        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }

        public override void Play(Facing face)
        {
            base.Play(face);
            _animator.SetTrigger("Enter");
        }
    }
}