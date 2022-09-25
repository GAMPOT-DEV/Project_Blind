using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class BatTest : CrowdTest
    {
        [SerializeField] private Transform AttackHitBoxRange;

        protected void Awake()
        {
            base.Awake();

            Data.sensingRange = new Vector2(12f, 8f);
            Data.speed = 0.1f ;
            Data.runSpeed = 0.07f;
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
            if (currentAttack == null)
            {
                if (Random.Range(0, 100) > 20)
                    currentAttack = "Basic Attack";
                else
                    currentAttack = "Skill Attack";
                Dischangeable();
                _anim.SetTrigger(currentAttack);
            }
        }

        public override void AttackHitBox()
        {
            col = gameObject.AddComponent<BoxCollider2D>();
            col.offset = new Vector2(_col.offset.x +3.5f, _col.offset.y);
            col.size = new Vector2(7, 9);
        }

        public void DeadSound()
        {
            SoundManager.Instance.Play("Crowd/Bat/Death");
        }
    }
}