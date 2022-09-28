using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind {
    public class ParasiteMonster : CrowdEnemyCharacter
    {
        protected void Awake()
        {
            base.Awake();
            _patrolTime = 2;
        }

        private void Start()
        {
            startingPosition = gameObject.transform;
            _attack.Init(13, 10);
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

        public void AniGrab()
        {
            if (Physics2D.OverlapCircle(gameObject.transform.position + new Vector3(11, 3, 0), 3f, 13))
            {
                Debug.Log("Grab Success");
                _anim.SetBool("Success", true);
            }
            else
            {
                Debug.Log("Grab Fail");
                _anim.SetBool("Fail", true);
            }
            _anim.SetBool("Grab Attack", false);
        }
        
        public override void AttackHitBox()
        {
            col = gameObject.AddComponent<BoxCollider2D>();
            col.offset = new Vector2(_col.offset.x +3.5f, _col.offset.y);
            col.size = new Vector2(13, 10);
            col.isTrigger = true;
        }
    }
}
