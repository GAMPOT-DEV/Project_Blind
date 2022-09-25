using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Blind
{
    public class CrowdTest : EnemyCharacter
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

        [SerializeField] protected State state = State.Patrol;
        private State preState = State.Die; //직전 상태

        protected float _patrolTime;
        protected Animator _anim;
        protected float _chaseRange = 20;

        [SerializeField] protected bool isChangeable = true;
        [SerializeField] protected bool isRestTime = false;
        public bool IsAttack = false;
        protected float afterDelayTime = 0.3f;
        [SerializeField] protected bool coolDown = false;
        [SerializeField] protected string currentAttack;

        protected Coroutine co_state;

        protected BoxCollider2D col;
        protected bool createAttackHitBox;

        protected void Awake()
        {
            base.Awake();
            state = State.Patrol;
            patrolDirection = new Vector2(RandomDirection() * Data.speed, 0);
            playerFinder.setRange(Data.sensingRange);
            attackSense = GetComponentInChildren<EnemyAttack>();
            _anim = GetComponent<Animator>();
            player = GameObject.FindGameObjectWithTag("Player");
            //attackSense.setRange(Data.attackRange);
        }

        protected virtual void FixedUpdate()
        {
            NextAction();

            //State 업데이트
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
            if (Hp.GetHP() <= 0)
            {
                Dischangeable();
                state = State.Die;
            }

            _characterController2D.OnFixedUpdate();
        }

        protected virtual void updatePatrol()
        {
            if(co_state == null)
            {
                co_state = StartCoroutine(CoPatrol(_patrolTime));
            }

            _anim.SetBool("Patrol", true);
            _characterController2D.Move(patrolDirection);
        }
        
        protected virtual void updateDefault()
        {
            if (co_state == null)
            {
                co_state = StartCoroutine(CoDefault());
            }
            _anim.SetBool("Default", true);
        }
        
        protected virtual void updateChase()
        {
            _anim.SetBool("Chase", true);
            _characterController2D.Move(playerFinder.ChasePlayer() * Data.runSpeed);
        }

        protected virtual void updateAttack()
        {
            throw new NotImplementedException();
        }

        protected virtual void updateStun()
        {
            if (co_state == null)
            {
                co_state = StartCoroutine(CoStun());
            }
            _anim.SetBool("Stun", true);
        }

        protected virtual void updateHitted()
        {
            _anim.SetTrigger("Hurt");
            if (Hp.GetHP() <= 0)
            {
                state = State.Die;
            }
            NextAction();
        }
        
        protected virtual void updateDie()
        {
            gameObject.layer = 16;
            _anim.SetBool("Dead", true);
            DeathCallback.Invoke();
        }

        protected virtual void updateAvoid()
        {
            throw new NotImplementedException();
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

        public virtual void OnStun()
        {
            state = State.Stun;
            currentAttack = null;
            Enchangeable();
            //StopCoroutine(co_state);
            Destroy(col);
        }

        protected override void onHurt()
        {
            //base.onHurt();
            
            _anim.SetTrigger("Hurt");
            Enchangeable();
            currentAttack = null;
            //state = State.Hitted;
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

        protected IEnumerator CoPatrol(float patrolTime)
        {
            yield return new WaitForSeconds(patrolTime);
            isRestTime = true;
        }

        public virtual IEnumerator CoStun()
        {
            yield return new WaitForSeconds(Data.stunTime);
            Enchangeable();
        }

        public IEnumerator CoDefault()
        {
            yield return new WaitForSeconds(1f);
            isRestTime = false;
            Flip();
        }

        protected IEnumerator CoolDown()
        {
            coolDown = true;
            yield return new WaitForSeconds(afterDelayTime);
            coolDown = false;
        }

        public virtual void AniAfterAttack()
        {
            Enchangeable();
            createAttackHitBox = false;
            currentAttack = null;
            Destroy(col);
            CoolDown();
        }

        public void AniParingenable()
        {
            if (!createAttackHitBox)
            {
                AttackHitBox();
                createAttackHitBox = true;
            }

            IsAttack = true;
        }

        public void AniAttackStart()
        {
            IsAttack = false;
            _attack.EnableDamage();
            Destroy(col);
        }

        public void AniAttackEnd()
        {
            _attack.DisableDamage();
            Destroy(col);
        }

        public void AniDestroy()
        {
            Destroy(gameObject, 1f);
        }

        protected void Enchangeable()
        {
            isChangeable = true;
        }

        protected void Dischangeable()
        {
            Debug.Log("Dischange");
            isChangeable = false;
        }

        protected virtual void NextAction()
        {
            if (isChangeable)
            {
                if (Physics2D.OverlapCircle(WallCheck.position, 0.01f, WallLayer))
                    state = State.Default;

                if (attackSense.Attackable())
                {
                    if (coolDown)
                        state = State.Default;
                    else
                        state = State.Attack;
                } 
                else if (playerFinder.FindPlayer())
                    state = State.Chase;
                else if (isRestTime)
                    state = State.Default;
                else
                    state = State.Patrol;
            }

            if(preState != state)
            {
                Debug.Log(state);
                preState = state;
                if (co_state != null)
                {
                    StopCoroutine(co_state);
                    co_state = null;
                }
                offAllAnimation();
                currentAttack = null;
            }
        }

        protected virtual void offAllAnimation()
        {
            _anim.SetBool("Patrol", false);
            _anim.SetBool("Default", false);
            _anim.SetBool("Chase", false);
            _anim.SetBool("Stun", false);
            _anim.SetBool("Attack", false);
            Enchangeable();
        }

        public virtual void AttackHitBox()
        {
            return;
        }
    }
}