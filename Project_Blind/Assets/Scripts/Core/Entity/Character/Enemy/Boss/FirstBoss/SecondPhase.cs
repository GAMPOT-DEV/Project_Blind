using System;
using UnityEngine;
using System.Collections;
using Random = System.Random;
namespace Blind
{
    public class SecondPhase : BossPhase
    {
        protected Random _rand = new Random();
        private Animator _animator;
        [SerializeField] private Transform point;

        public void Init(FirstBossEnemy firstBossEnemy)
        {
            _parent = firstBossEnemy;
            _animator = _parent.gameObject.GetComponent<Animator>();
        }

        public float ReturnHp()
        {
            return _parent.Hp.GetHP();
        }
        
        public override void Play()
        {
            
        }

        public override void End()
        {
        }

        public override void Stop()
        {
            
        }

        public void StartPattern(int pattern)
        {
            switch (pattern)
            {
                case 1:
                    Pattern1();
                    break;
                case 2:
                    Pattern2();
                    break;
                case 3:
                    Pattern3();
                    break;
                case 4:
                    Pattern4();
                    break;
            }
        }

        public int Range()
        {
            var rand = _rand.Next(1, 5);
            return rand;
        }

        public void Pattern1()
        {
            _animator.SetTrigger("Attack");
        }

        public void Pattern2()
        {
            _animator.SetTrigger("Attack2");
        }

        public void Pattern3()
        {
            var next = _rand.Next(0, 3);
            if (next == 0)
                _animator.SetTrigger("biteL");
            else if(next == 1)
                _animator.SetTrigger("biteR");
            else
                _animator.SetTrigger("bite");
            
        }

        public void Pattern4()
        {
            _animator.SetTrigger("shout");
        }

        public void Parry()
        {
            _animator.SetTrigger("parry");
        }

        public void Dead()
        {
            _animator.SetBool("Dead", true);
        }
    }
}