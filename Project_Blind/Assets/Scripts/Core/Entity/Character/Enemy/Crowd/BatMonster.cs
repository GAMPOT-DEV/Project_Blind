using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class BatMonster : CrowdEnemyCharacter
    {
        [SerializeField] private Transform AttackHitBoxRange;

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
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

        public override void AttackHitBox()
        {
            col = gameObject.AddComponent<BoxCollider2D>();
            col.offset = new Vector2(_col.offset.x + 3.5f, _col.offset.y);
            col.size = new Vector2(7, 10);
            col.isTrigger = true;
        }

        public void DeadSound()
        {
            SoundManager.Instance.Play("Crowd/Bat/Death");
        }

        public void AttackSound()
        {
            if(currentAttack == 1)
                SoundManager.Instance.Play("Crowd/Bat/SmallAttack");
            else if(currentAttack == 2)
                SoundManager.Instance.Play("Crowd/Bat/BigAttack");
        }

        public override void WalkSound()
        {
            SoundManager.Instance.Play("Crowd/Bat/Patrol");
        }
    }
}