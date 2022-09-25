using System;
using System.Collections;
using UnityEngine;

namespace Blind
{
    /// <summary>
    /// 물기 패턴
    /// </summary>
    public class BossPattern4: BossAttackPattern<FirstBossEnemy>
    {
        [SerializeField] private float damage = 1f;
        [SerializeField] private int MaxHitCount = 1;
        [SerializeField] private float AttackTime = 1f;
        private int _hitCount = 0;
        
        private Vector2 _originPos;
        private Vector2 _playerPos;

        private Chin _upperChin;
        private Chin _lowerChin;

        private void Awake()
        {
            _upperChin = transform.GetChild(0).GetComponent<Chin>();
            _lowerChin = transform.GetChild(1).GetComponent<Chin>();
        }

        public void Start()
        {
            var originTransform = transform.position;
            _originPos = new Vector2(originTransform.x,originTransform.y); // 기존 포지션 저장
            
            var playerTransform = GameManager.Instance.Player.transform.position;
            _playerPos = new Vector2(playerTransform.x,playerTransform.y); // 플레이어의 현재 포지션 저장
        }

        public override Coroutine AttackPattern()
        {
            return StartCoroutine(attackPattern());
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            var character = col.GetComponent<Character>();
            if (character != null && character is PlayerCharacter)
            {
                onHitPlayer(character as PlayerCharacter);
            }
        }

        private void onHitPlayer(PlayerCharacter player)
        {
            if (MaxHitCount <= _hitCount) return;
            Facing face = player.gameObject.transform.position.x < transform.position.x
                ? Facing.Left
                : Facing.Right;
            player.HittedWithKnockBack(new AttackInfo(damage,face));
            _hitCount++;
        }

        private IEnumerator attackPattern()
        {
            var playerTrnasform = GameManager.Instance.Player.transform.position;
            _playerPos = new Vector2(playerTrnasform.x,playerTrnasform.y); // 플레이어의 현재 포지션 저장
            var position = transform.position;
            var currentPos = new Vector2(position.x, position.y);
            
            var pathPos = (_playerPos - currentPos)/AttackTime; // 이동시키기 위한 벡터
            
            float curTime = 0;
            
            while (curTime < AttackTime)
            {
                curTime += Time.deltaTime;
                var nowPos = currentPos + (pathPos * curTime); 
                transform.position = new Vector3(nowPos.x,nowPos.y,transform.position.z); 
                yield return null;
            }
            transform.position = _originPos;
            yield return null;
        }
    }
}