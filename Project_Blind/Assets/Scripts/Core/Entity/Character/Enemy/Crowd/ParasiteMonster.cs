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
            _attack.Init(7, 8);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        protected override void updateAttack()
        {
            if (_anim.GetBool("Basic Attack") == false && _anim.GetBool("Skill Attack") == false)
            {
                /*
                if (!createAttackHitBox)
                {
                    AttackHitBox();
                    createAttackHitBox = true;
                }*/
                
                if (Random.Range(0, 100) > 20)
                {
                    _anim.SetBool("Basic Attack", true);
                }
                else
                {
                    _anim.SetBool("Skill Attack", true);
                }
            }
        }
        /*
        public void AttackHitBox()
        {
            Debug.Log("dd");
            col = gameObject.AddComponent<BoxCollider2D>();
            col.offset = new Vector2(_col.offset.x +3.5f, _col.offset.y);
            col.size = new Vector2(7, 8);
        }*/
    }
}
