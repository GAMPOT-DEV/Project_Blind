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
            AttackStandby,
            Hitted,
            Stun,
            Avoid,
            Die,
            Test
        }

        protected State state;

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

                case State.AttackStandby:
                    updateAttackStandby();
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
            throw new NotImplementedException();
        }
        
        protected virtual void updateDefault()
        {
            throw new NotImplementedException();
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
            playerFinder.setRange(_sensingRange);
            attackSense = GetComponentInChildren<EnemyAttack>();
            attackSense.setRange(_attackRange);
            patrolDirection = new Vector2(RandomDirection() * _speed, 0f);
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
                patrolDirection = new Vector2(-_speed, 0f);
                //_sprite.flipX = false;
            }
            else
            {
                thisScale.x = Mathf.Abs(thisScale.x);
                patrolDirection = new Vector2(_speed, 0f);
                //_sprite.flipX = true;
            }
            transform.localScale = thisScale;
            _unitHPUI.Reverse();
        }

        protected override IEnumerator CoHitted()
        {
            yield return new WaitForSeconds(0.1f);
            state = State.Test;
        }
    }
}