using System;
using System.Collections;
using System.Security.Cryptography;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Blind
{
    public class BossHand: MonoBehaviour
    {
        private SpriteRenderer sprite;
        private BoxCollider2D _collider;
        public Transform StartTransform;
        public Vector2 EndTransform;
        private Vector2 TargetPostion;
        private bool isRight;
        private Facing facing;
        private bool isStop;
        private bool isParing = false;
        private bool isCameraShakeStop = false; // ���� 1: true, ���� 2: false
        private CinemachineImpulseSource _source;
        private bool isBossPatternCheck;
        private bool isPatternCheck;
        public void Awake()
        {
            sprite = GetComponent<SpriteRenderer>();
            _collider = GetComponent<BoxCollider2D>();
            _source = GetComponent<CinemachineImpulseSource>();
        }

        public void FixedUpdate()
        {
            if (!isStop)
            {
                if(isPatternCheck)
                    transform.position = Vector2.MoveTowards(transform.position, EndTransform, 1f);
                else
                {
                    transform.position = Vector2.MoveTowards(transform.position, EndTransform, 0.5f);
                }
                if(!isBossPatternCheck)_source.GenerateImpulse();
                if (transform.position.x == EndTransform.x)
                {
                    if (isBossPatternCheck) _source.GenerateImpulse();
                    isCameraShakeStop = true;
                    StartCoroutine(ObjectFade());
                    Destroy(gameObject);
                }
            }
            else
            {
                if (!isParing)
                {
                    float move = 5f * (float)facing;
                    TargetPostion = new Vector2(transform.position.x + move, transform.position.y);
                    isParing = true;
                    Debug.Log(transform.position.x + " " + TargetPostion.x);
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
                this.facing = Facing.Left;
            }
            else
            {
                this.facing = Facing.Right;
            }
        }
        public void CheckBossPattern(bool isBossPattern)
        {
            isBossPatternCheck = isBossPattern;
        }

        public void GetTransform(Vector2 left, Vector2 right, bool isPatternCheck)
        {
            this.isPatternCheck = isPatternCheck;
            transform.position = left;
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
                    SoundManager.Instance.Play("타격-무겁게", Define.Sound.Effect);
                    _player.HitWithKnockBack(new AttackInfo(1f,facing));
                    StartCoroutine(ResetTrigger());
                }
            }
        }

        IEnumerator ObjectFade()
        {
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(ObjectFadeManager.s_Instance.FadeOut());
        }

        public void Paring()
        {
            isStop = true;
        }
    }
}