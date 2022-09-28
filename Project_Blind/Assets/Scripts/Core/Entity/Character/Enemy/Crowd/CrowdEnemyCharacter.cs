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

        [SerializeField] protected State state;
        protected float _patrolTime;
        protected Animator _anim;
        protected float _chaseRange = 20;
        protected float afterDelayTime = 0.6f;
        protected bool attackable = true;
        protected int currentAttack = 0;

        public float CurrentStunGauge = 0;
        public float MaxStunGauge = 10f;
        public bool IsAttack = false;

        private Coroutine co_patrol;
        protected Coroutine co_stun;
        private Coroutine co_default;

        private State tmp = State.Die;
        protected BoxCollider2D col;
        protected bool createAttackHitBox;
        protected bool onSound = false;

        protected override void Awake()
        {
            base.Awake();
            state = State.Patrol;
            patrolDirection = new Vector2(RandomDirection() * Data.speed, 0);
            playerFinder.setRange(Data.sensingRange);
            attackSense = GetComponentInChildren<EnemyAttack>();
            _anim = GetComponent<Animator>();  
            //attackSense.setRange(Data.attackRange);
        }

        protected void Start()
        {
            SceneLinkedSMB<CrowdEnemyCharacter>.Initialise(_anim, this);
            player = GameObject.FindGameObjectWithTag("Player");
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

            if (Hp.GetHP() <= 0)
                state = State.Die;

            _characterController2D.OnFixedUpdate();
        }

        protected virtual void updatePatrol()
        {
            if (attackSense.Attackable() && attackable)
            {
                SoundManager.Instance.StopEffect();
                state = State.Attack;
                return;
            }

            if (playerFinder.FindPlayer())
            {
                SoundManager.Instance.StopEffect();
                state = State.Chase;
                if (co_patrol != null)
                {
                    StopCoroutine(co_patrol);
                    co_patrol = null;
                }
                return;
            }

            if (Physics2D.OverlapCircle(WallCheck.position, 0.01f, WallLayer))
            {
                state = State.Default;
                if (co_patrol != null) StopCoroutine(co_patrol);
                co_patrol = null;
                return;
            }

            if (co_patrol == null)
            {
                co_patrol = StartCoroutine(CoPatrol(_patrolTime));
            }

            _anim.SetInteger("State", 1);
            _characterController2D.Move(patrolDirection);
        }

        protected virtual void updateDefault()
        {
            if (playerFinder.FindPlayer())
            {
                state = State.Chase;
                return;
            }

            if (attackSense.Attackable() && attackable)
            {
                state = State.Attack;
                return;
            }

            if (co_default == null)
            {
                co_default = StartCoroutine(CoDefault());
            }

            _anim.SetInteger("State", 0);
        }
        
        protected virtual void updateChase()
        {
            flipToFacing();

            if (playerFinder.MissPlayer())
            {
                state = State.Patrol;
                onSound = false;
                return;
            }

            if (attackSense.Attackable() && attackable)
            {
                state = State.Attack;
                onSound = false;
                return;
            }

            if (!onSound)
            {
                onSound = true;
                WalkSound();
            }

            _anim.SetInteger("State", 2);
            _characterController2D.Move(playerFinder.ChasePlayer() * Data.runSpeed);
        }

        protected virtual void updateAttack()
        {
            throw new NotImplementedException();
        }

        protected virtual void updateStun()
        {
            if (co_stun == null)
            {
                co_stun = StartCoroutine(CoStun());
            }
            _anim.SetInteger("State", 5);
        }

        protected virtual void updateHitted()
        {
            _anim.SetTrigger("Hurt");
            onSound = false;
            NextAction();
        }
        
        protected virtual void updateAttackStandby()
        {
            throw new NotImplementedException();
        }
        
        protected virtual void updateDie()
        {
            gameObject.layer = 16;
            _anim.SetInteger("State", 6);
            DeathCallback.Invoke();
            Destroy(gameObject, 3f);
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
            Destroy(col);
        }

        protected override void onHurt()
        {
            base.onHurt();
            _anim.SetTrigger("Hurt");
            flipToFacing();
            HurtMove(GetFacing());
            StartCoroutine(onHurtAnim());
            if (co_stun != null)
            {
                StopCoroutine(co_stun);
            }
        }

        protected void flipToFacing()
        {
            if (player == null)
                player = GameObject.FindGameObjectWithTag("Player");

            switch (GetFacing())
            {
                case Facing.Left:
                    if (player.transform.position.x > transform.position.x)
                        Flip();
                    break;

                case Facing.Right:
                    if (player.transform.position.x < transform.position.x)
                        Flip();
                    break;
            }
        }

        private IEnumerator onHurtAnim()
        {
            Debug.Log(_renderer.material);
            _renderer.material.SetFloat("EnableHit",1);
            yield return new WaitForSeconds(1f);
            _renderer.material.SetFloat("EnableHit",0);
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
            SoundManager.Instance.StopEffect();
            state = State.Default;
            co_patrol = null;
        }

        public virtual IEnumerator CoStun()
        {
            yield return new WaitForSeconds(Data.stunTime);
            NextAction();

            co_stun = null;
        }

        public IEnumerator CoDefault()
        {
            yield return new WaitForSeconds(1f);
            state = State.Patrol;
            Flip();
            co_default = null;
        }

        public virtual void AniAfterAttack()
        {
            NextAction();

            createAttackHitBox = false;
            currentAttack = 0;
            Destroy(col);
            attackable = false;
            StartCoroutine(Delay());
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
            attackable = false;
            IsAttack = false;
            _attack.EnableDamage();
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

        protected IEnumerator Delay()
        {
            yield return new WaitForSeconds(afterDelayTime);
            attackable = true;
        }

        protected virtual void NextAction()
        {
            if (attackSense.Attackable())
            {
                if (attackable)
                    state = State.Attack;
                else
                    state = State.Default;
            }
            else if (playerFinder.FindPlayer())
                state = State.Chase;
            else
                state = State.Patrol;
        }

        public virtual void AttackHitBox()
        {
            return;
        }

        public virtual void WalkSound()
        {
            return;
        }
    }
}