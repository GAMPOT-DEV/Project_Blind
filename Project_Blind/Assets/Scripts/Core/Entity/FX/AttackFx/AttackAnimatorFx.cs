using UnityEngine;

namespace Blind 
{
    [RequireComponent(typeof(Animator))]
    public class AttackAnimatorFx : AttackFX
    {
        private Animator _animator;
        private static readonly int Play1 = Animator.StringToHash("Play");

        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }

        public virtual void Play(Facing face)
        {
            base.Play(face);
            _animator.Play(Play1,-1,0);
        }
        
    }
}