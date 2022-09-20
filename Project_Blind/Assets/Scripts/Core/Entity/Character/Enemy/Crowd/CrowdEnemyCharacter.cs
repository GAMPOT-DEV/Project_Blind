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
        protected float _patrolTime;
        protected Animator _anim;
        private int defaultCount = 0;

        public float CurrentStunGauge = 0;
        public float MaxStunGauge = 10f;
        public bool isPowerAttack = false;
        public bool IsAttack = false;

        private Coroutine co_patrol;
        private Coroutine co_stun;

        protected void Awake()
        {
            base.Awake();
            state = State.Patrol;
            patrolDirection = new Vector2(RandomDirection() * Data.speed, 0);
            playerFinder.setRange(Data.sensingRange);
            attackSense = GetComponentInChildren<EnemyAttack>();
            _anim = GetComponent<Animator>();
            attackSense.setRange(Data.attackRange);
        }

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
            if (_anim.GetBool("Chase") == false)
            {
                _anim.SetBool("Chase", true);
            }

            if (playerFinder.MissPlayer())
            {
                state = State.Patrol;
                _anim.SetBool("Chase", false);
                return;
            }

            if (attackSense.Attackable())
            {
                state = State.Attack;
                _anim.SetBool("Chase", false);
                return;
            }

            _characterController2D.Move(playerFinder.ChasePlayer() * Data.runSpeed);
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

        public void OnHurt()
        {
            base.onHurt();
            _anim.SetBool("Hurt", true);
            Hp.GetDamage(1f);
            if (Hp.GetHP() <= 0)
                _anim.SetBool("Dead", true);
            if (isPowerAttack)
                CurrentStunGauge += 2.5f;
            else
                CurrentStunGauge += 1f;
        }

        protected void Flip()
        {
            Vector2 thisScale = transform.localScale;
            if (patrolDirection.x >= 0)
            {
                thisScale.x = -Mathf.Abs(thisScale.x);
                patrolDirection = new Vector2(-Data.speed, 0f);
            }
            else
            {
                thisScale.x = Mathf.Abs(thisScale.x);
                patrolDirection = new Vector2(Data.speed, 0f);
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

        public IEnumerator CoStun()
        {
            _anim.SetBool("Stun", true);
            yield return new WaitForSeconds(Data.stunTime);

            if (attackSense.Attackable())
                state = State.Attack;
            else if (playerFinder.FindPlayer())
                state = State.Chase;
            else
                state = State.Default;

            co_stun = null;
            _anim.SetBool("Stun", false);
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

        public void AniAfterAttack()
        {
            if (attackSense.Attackable())
                state = State.Attack;
            else if (playerFinder.FindPlayer())
                state = State.Chase;
            else
                state = State.Default;
            _anim.SetBool("Basic Attack", false);
            _anim.SetBool("Skill Attack", false);
        }

        public void AniAttackStart()
        {
            _attack.EnableDamage();
            IsAttack = true;
        }

        public void AniAttackEnd()
        {
            _attack.DisableDamage();
            IsAttack = false;
        }
    }
}