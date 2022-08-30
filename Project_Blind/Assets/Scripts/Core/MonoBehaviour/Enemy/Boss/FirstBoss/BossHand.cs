using System;
using System.Collections;
using UnityEngine;

namespace Blind
{
    public class BossHand: MonoBehaviour
    {
        private SpriteRenderer sprite;
        private BoxCollider2D _collider;
        public Transform StartTransform;
        public Transform EndTransform;
        private bool isRight;
        private int speed = 2;
        private int facing;
        public bool isMove;
        public void Awake()
        {
            sprite = GetComponent<SpriteRenderer>();
            _collider = GetComponent<BoxCollider2D>();
        }

        public void FixedUpdate()
        {
            transform.position = Vector2.MoveTowards(transform.position, EndTransform.position, 0.5f);
            if(transform.position.x == EndTransform.position.x) Destroy(gameObject);
        }

        public void GetFacing(int facing)
        {
            if (facing == 0)
            {
                sprite.flipX = false;
                this.facing = 1;
            }
            else
            {
                sprite.flipX = true;
                this.facing = -1;
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
                Debug.Log("DD");
                PlayerCharacter _player = col.gameObject.GetComponent<PlayerCharacter>();
                if (!_player._isInvincibility)
                {
                    _collider.isTrigger = true;
                    _player.HpCenter.GetDamage(5f);
                    _player.OnHurt();
                    _player.HurtMove(_player._hurtMove * facing);
                    StartCoroutine(ResetTrigger());
                }
            }
        }
    }
}