using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class BatMonster : CrowdEnemyCharacter
    {
        [SerializeField] private Transform AttackHitBoxRange;

        protected void Awake()
        {
            base.Awake();

            Data.sensingRange = new Vector2(12f, 8f);
            Data.speed = 0.3f;
            Data.runSpeed = 0.33f;
            Data.attackCoolTime = 0.5f;
            Data.attackSpeed = 0.3f;
            Data.attackRange = new Vector2(9f, 8f);
            Data.stunTime = 1.5f;
            _patrolTime = 3f;
        }

        private void Start()
        {
            startingPosition = gameObject.transform;
            _attack.Init(7, 10);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        protected override void updateAttack()
        {
            if (currentAttack == 0)
            {
                if (Random.Range(0, 100) > 20)
                    currentAttack = 1;
                else
                    currentAttack = 2;

            }

            flipToFacing();
            if (currentAttack == 1)
                _anim.SetInteger("State", 30);
            else if (currentAttack == 2)
                _anim.SetInteger("State", 31);
        }

        public void DeadSound()
        {
            SoundManager.Instance.Play("Crowd/Bat/Death");
        }

        public override void AttackHitBox()
        {
            col = gameObject.AddComponent<BoxCollider2D>();
            col.offset = new Vector2(_col.offset.x + 3.5f, _col.offset.y);
            col.size = new Vector2(7, 10);
            col.isTrigger = true;
        }
    }
}