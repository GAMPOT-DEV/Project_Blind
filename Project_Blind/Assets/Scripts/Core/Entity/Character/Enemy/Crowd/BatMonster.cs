using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class BatMonster : CrowdEnemyCharacter
    {
        private Transform knockBackRange;

        private Coroutine Co_default;
        private Coroutine Co_attack;
        private Coroutine Co_attackStandby;
        private Coroutine Co_hitted;
        private Coroutine Co_stun;
        private Coroutine Co_die;
        
        public int StunGauge;
        public int maxStunGauge;

        public bool isPowerAttack;
        protected void Awake()
        {
            base.Awake();
            Data.sensingRange = new Vector2(10f, 5f);
            Data.speed = 0.1f ;
            Data.runSpeed = 0.07f;
            Data.attackCoolTime = 0.5f;
            Data.attackSpeed = 0.3f;
            Data.attackRange = new Vector2(1.5f, 2f);
            Data.stunTime = 1f;

        }

        private void Start()
        {
            startingPosition = gameObject.transform;
            _attack.Init(2, 2);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        protected override void updatePatrol()
        {
            if (playerFinder.FindPlayer())
            {
                state = State.Chase;
                return;
            }
            if (Physics2D.OverlapCircle(WallCheck.position, 0.01f, WallLayer))
            {
                state = State.Default;
                return;
            }

            _characterController2D.Move(patrolDirection);
        }

        protected override void updateDefault()
        {
            if (Co_default == null)
                Co_default = StartCoroutine(CoWaitDefalut(1f));

            if (playerFinder.FindPlayer())
            {
                StopCoroutine(Co_default);
                Co_default = null;
                state = State.Chase;
                return;
            }
        }

        protected override void updateChase()
        {
            if (playerFinder.MissPlayer())
            {
                state = State.Patrol;
                return;
            }

            if (attackSense.Attackable())
            {
                state = State.AttackStandby;
                return;
            }

            _characterController2D.Move(playerFinder.ChasePlayer() * Data.runSpeed);
        }

        protected override void updateAttack()
        {
            if (Co_attack == null)
            {
                Co_attack = StartCoroutine(CoAttack());
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }

        protected override void updateAttackStandby()
        {
            if (Co_attackStandby == null)
            {
                Co_attackStandby = StartCoroutine(CoAttackStandby(Data.attackSpeed));
                gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
        }

        protected override void updateHitted()
        {

            if (Co_hitted == null)
            {
                StopAllCoroutines();
                StartCoroutine(CoHitted());
            }

            Vector2 hittedVelocity = Vector2.zero;
            if (playerFinder.ChasePlayer().x > 0) //플레이어가 오른쪽
            {
                hittedVelocity = new Vector2(-0.2f, 0);
            }
            else
            {
                hittedVelocity = new Vector2(0.2f, 0);
            }

            _characterController2D.Move(hittedVelocity);

            /*
            if (playerFinder.FindPlayer())
                state = State.Chase;
            else if (attackSense.Attackable())
                state = State.AttackStandby;
            else
                state = State.Patrol;
            */
        }

        protected override void updateStun()
        {
            StopAllCoroutines();
            Co_stun = StartCoroutine(CoStun());
        }

        protected override void updateDie()
        {
            Co_die = StartCoroutine(CoDie());
        }

        public bool isAttack()
        {
            if (state == State.Attack)
                return true;
            else
                return false;
        }

        private IEnumerator CoWaitDefalut(float time)
        {
            yield return new WaitForSeconds(time);
            Flip();
            state = State.Patrol; 
            Co_default = null;
        }

        private IEnumerator CoAttackStandby(float time)
        {
            yield return new WaitForSeconds(time);
            state = State.Attack;
            Co_attackStandby = null;
        }

        private IEnumerator CoAttack()
        {
            yield return new WaitForSeconds(2f);
            Debug.Log("공격!!");
            _attack.EnableDamage();
            gameObject.GetComponent<SpriteRenderer>().color = Color.blue; //000FE9
            yield return new WaitForSeconds(0.2f);

            _attack.DisableDamage();
            if (attackSense.Attackable())
                state = State.AttackStandby;
            else if (playerFinder.FindPlayer())
                state = State.Chase;
            else
                state = State.Default;
            Co_attack = null;
        }

        private IEnumerator CoStun()
        {
            yield return new WaitForSeconds(Data.stunTime);

            if (attackSense.Attackable())
                state = State.AttackStandby;
            else if (playerFinder.FindPlayer())
                state = State.Chase;
            else
                state = State.Patrol;

            Co_stun = null;
        }

        private IEnumerator CoDie()
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

        private IEnumerator CoHitted()
        {
            yield return null;
            if (playerFinder.FindPlayer())
                state = State.Chase;
            else if (attackSense.Attackable())
                state = State.AttackStandby;
            else
                state = State.Patrol;

            Co_hitted = null;
        }
    }
}