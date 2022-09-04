using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Blind
{
    public class BossHand: MonoBehaviour
    {
        private SpriteRenderer sprite;
        private BoxCollider2D _collider;
        public Transform StartTransform;
        public Transform EndTransform;
        private Vector2 TargetPostion;
        private bool isRight;
        private int speed = 2;
        private Facing facing;
        private bool isStop;
        private bool isParing = false;
        private bool isCameraShakeStop = false;
        public void Awake()
        {
            sprite = GetComponent<SpriteRenderer>();
            _collider = GetComponent<BoxCollider2D>();
        }

        public void FixedUpdate()
        {
            if (!isStop)
            {
                transform.position = Vector2.MoveTowards(transform.position, EndTransform.position, 0.5f);
                if (transform.position.x == EndTransform.position.x)
                {
                    isCameraShakeStop = true;
                    Destroy(gameObject);
                }
            }
            else
            {
                if (!isParing)
                {
                    TargetPostion = new Vector2(transform.position.x + (2f * (float)facing), transform.position.y);
                    isParing = true;
                }
                transform.position = Vector2.MoveTowards(transform.position,
                    TargetPostion, 0.1f);

                if (transform.position.x == TargetPostion.x)
                {
                    Destroy(gameObject);
                    isStop = false;
                    isCameraShakeStop = true;
                }
            }
        }

        public void GetFacing(bool facing)
        {
            if (facing)
            {
                sprite.flipX = false;
                this.facing = Facing.Right;
            }
            else
            {
                sprite.flipX = true;
                this.facing = Facing.Left;
            }
        }

        public void GetTransform(Transform left, Transform right)
        {
            transform.position = left.position;
            EndTransform = right;
        }

        IEnumerator ResetTrigger()
        {
            yield return new WaitForSeconds(2f);
            _collider.isTrigger = false;
        }

        public void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.tag.Equals("Player"))
            {
                PlayerCharacter _player = col.gameObject.GetComponent<PlayerCharacter>();
                if (!_player._isInvincibility)
                {
                    _collider.isTrigger = true;
                    _player.HittedWithKnockBack(new AttackInfo(5f,facing));
                    StartCoroutine(ResetTrigger());
                }
            }
        }
        

        public void Paring()
        {
            isStop = true;
        }
    }
}