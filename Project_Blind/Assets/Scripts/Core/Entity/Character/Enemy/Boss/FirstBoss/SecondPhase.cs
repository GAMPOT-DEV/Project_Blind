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
        private Coroutine _coroutine;

        public void Init(FirstBossEnemy firstBossEnemy)
        {
            _parent = firstBossEnemy;
            _animator = _parent.gameObject.GetComponent<Animator>();
            Debug.Log("dd");
        }

        public float ReturnHp()
        {
            return _parent.Hp.GetHP();
        }
        
        public override void Play()
        {
            _coroutine = StartCoroutine(StartPattern());
        }

        public override void End()
        {
        }

        public override void Stop()
        {
            StopCoroutine(_coroutine);   
        }

        private void Setting()
        {
            _animator = _parent.gameObject.GetComponent<Animator>();
        }

        public IEnumerator StartPattern()
        {
            Setting();
            yield return new WaitForSeconds(3f);
            while (true)
            {
                int pattern = Range();
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

                yield return new WaitForSeconds(3f);
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
            
            _parent.Pattern2Start(next);
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