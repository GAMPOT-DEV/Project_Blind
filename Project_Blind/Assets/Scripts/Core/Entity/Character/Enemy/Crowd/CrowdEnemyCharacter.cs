using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Blind
{
    public class CrowdEnemyCharacter : EnemyCharacter
    {
        protected enum State
        {
            Patrol,
            Default,
            Chase,
            Attack,
            Hitted,
            Stun,
            Avoid,
            Die,
            Test
        }

        protected State state;
        protected float _patrolTime = 3f;
        protected Animator _anim;
        private int defaultCount = 0;

        private Coroutine co_patrol;

        protected virtual void FixedUpdate()
        {
            switch (state)
            {
                case State.Patrol:
                    updatePatrol();
                    break;

                case State.Default:
                    updateDefault();
                    break;

                case State.Chase:
                    updateChase();
                    break;

                case State.Attack:
                    updateAttack();
                    break;

                case State.Hitted:
                    updateHitted();
                    break;

                case State.Stun:
                    updateStun();
                    break;

                case State.Die:
                    updateDie();
                    break;
                case State.Avoid:
                    updateAvoid();
                    break;
            }
            _characterController2D.OnFixedUpdate();
            //체력 업데이트
            if (Hp.GetHP() <= 0)
                state = State.Die;
        }

        protected virtual void updatePatrol()
        {
            if (_anim.GetBool("Patrol") == false)
            {
                _anim.SetBool("Patrol", true);
            }

            if (playerFinder.FindPlayer())
            {
                state = State.Chase;
                StopCoroutine(co_patrol);
                co_patrol = null;
                _anim.SetBool("Patrol", false);
                return;
            }
            if (Physics2D.OverlapCircle(WallCheck.position, 0.01f, WallLayer))
            {
                state = State.Default;
                StopCoroutine(co_patrol);
                co_patrol = null;
                _anim.SetBool("Patrol", false);
                return;
            }

            _characterController2D.Move(patrolDirection);

            if (co_patrol == null)
                co_patrol = StartCoroutine(CoPatrol(_patrolTime));
        }
        
        protected virtual void updateDefault()
        {
            if (_anim.GetBool("Default") == false)
            {
                _anim.SetBool("Default", true);
            }

            if (playerFinder.FindPlayer())
            {
                state = State.Chase;
                _anim.SetBool("Default", false);
                return;
            }
        }
        
        protected virtual void updateChase()
        {
            throw new NotImplementedException();
        }

        protected virtual void updateAttack()
        {
            throw new NotImplementedException();
        }
        protected virtual void updateStun()
        {
            throw new NotImplementedException();
        }
        protected virtual void updateHitted()
        {
            throw new NotImplementedException();
        }
        
        protected virtual void updateAttackStandby()
        {
            throw new NotImplementedException();
        }
        
        protected virtual void updateDie()
        {
            throw new NotImplementedException();
        }

        protected virtual void updateAvoid()
        {
            return;
        }

        protected void Awake()
        {
            base.Awake(); 
            state = State.Patrol;
            playerFinder.setRange(Data.sensingRange);
            attackSense = GetComponentInChildren<EnemyAttack>();
            _anim = GetComponent<Animator>();
            attackSense.setRange(Data.attackRange);
        }
        
        protected int RandomDirection()
        {
            int RanNum = Random.Range(0, 100);
            if (RanNum > 50)
                return 1;
            else
            {
                Flip();
                return -1;
            }
        }
        
        protected void Flip()
        {
            Vector2 thisScale = transform.localScale;
            if (patrolDirection.x >= 0)
            {
                thisScale.x = -Mathf.Abs(thisScale.x);
                patrolDirection = new Vector2(-Data.speed, 0f);
                //_sprite.flipX = false;
            }
            else
            {
                thisScale.x = Mathf.Abs(thisScale.x);
                patrolDirection = new Vector2(Data.speed, 0f);
                //_sprite.flipX = true;
            }
            transform.localScale = thisScale;
            _unitHPUI.Reverse();
        }

        public bool isAttack()
        {
            if (state == State.Attack)
                return true;
            else
                return false;
        }

        protected override IEnumerator CoHitted()
        {
            yield return new WaitForSeconds(0.1f);
            state = State.Test;
        }

        protected IEnumerator CoPatrol(float patrolTime)
        {
            yield return new WaitForSeconds(patrolTime);
            state = State.Default;
            co_patrol = null;
            animChange("Patrol", "Default");
        }

        protected IEnumerator CoDie()
        {
            yield return new WaitForSeconds(1);
            while (_sprite.color.a > 0)
            {
                var color = _sprite.color;
                color.a -= (.25f * Time.deltaTime);

                _sprite.color = color;
                yield return null;
            }
            Destroy(gameObject);
        }

        protected void animChange(string before, string after)
        {
            _anim.SetBool(before, false);
            _anim.SetBool(after, true);
        }

        public void AniDefault()
        {
            defaultCount++;
            if (defaultCount == 2)
            {
                _anim.SetBool("Default", false);
                state = State.Patrol;
                Flip();
                defaultCount = 0;
            }
        }
    }
}