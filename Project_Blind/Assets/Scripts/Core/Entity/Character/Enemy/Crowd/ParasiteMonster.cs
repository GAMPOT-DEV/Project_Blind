using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind {
    public class ParasiteMonster : CrowdEnemyCharacter
    {
        private Coroutine Co_attack;
        private Coroutine Co_hitted;
        private Coroutine Co_stun;
        private Coroutine Co_die;

        public int StunGauge;
        public int maxStunGauge;

        public bool isPowerAttack;


        protected void Awake()
        {
            base.Awake();

            Data.sensingRange = new Vector2(12f, 8f);
            Data.speed = 0.1f;
            Data.runSpeed = 0.07f;
            Data.attackCoolTime = 0.5f;
            Data.attackSpeed = 0.3f;
            Data.attackRange = new Vector2(9f, 8f);
            Data.stunTime = 1f;
            _patrolTime = 2;

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

        protected override void updateChase()
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
                //attackStandby가 꼭 필요할까...?
                //state = State.AttackStandby;
                state = State.Attack;
                _anim.SetBool("Chase", false);
                return;
            }

            _characterController2D.Move(playerFinder.ChasePlayer() * Data.runSpeed);
        }

        protected override void updateAttack()
        {
            if (Co_attack == null)
            {
                Co_attack = StartCoroutine(CoAttack());
            }

            if (_anim.GetBool("Basic Attack") == false)
            {
                _anim.SetBool("Basic Attack", true);
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

        private IEnumerator CoAttack()
        {
            //yield return new WaitForSeconds(2f);
            Debug.Log("공격!!");
            _attack.EnableDamage();
            yield return new WaitForSeconds(0.2f);
            _attack.DisableDamage();

            Co_attack = null;
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

            Co_stun = null;
            _anim.SetBool("Stun", false);
        }
    }
}
