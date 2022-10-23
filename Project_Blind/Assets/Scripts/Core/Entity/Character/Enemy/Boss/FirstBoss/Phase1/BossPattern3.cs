using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Blind
{
    /// <summary>
    /// 물기 패턴
    /// </summary>
    public class BossPattern3: BossAttackPattern<FirstBossEnemy>
    {
        [SerializeField] private float damage = 1f;
        private Chin _upperChin;
        private Chin _lowerChin;
        private int _attackAbleCount = 1;
        private Vector2 centerPosiotion;

        private void Awake()
        {
            _upperChin = transform.GetChild(0).GetComponent<Chin>();
            _lowerChin = transform.GetChild(1).GetComponent<Chin>();
            _upperChin.Parent = this;
            _lowerChin.Parent = this;
        }

        public override Coroutine AttackPattern()
        {
            _attackAbleCount = 1;
            transform.position = GameManager.Instance.Player.transform.position;
            _upperChin.gameObject.SetActive(true);
            _lowerChin.gameObject.SetActive(true);
            centerPosiotion = new Vector2(_upperChin.transform.position.x,
                (_upperChin.transform.position.y + _lowerChin.transform.position.y) / 2);
            _upperChin.Play(centerPosiotion);
            _lowerChin.Play(centerPosiotion);
            return End();
        }

        Coroutine End()
        {
            return StartCoroutine(realEnd());
        }

        IEnumerator realEnd()
        {
            yield return new WaitForSeconds(3f);

        }

        public void Attack()
        {
            if (_attackAbleCount == 0) return;
            GameManager.Instance.Player.Hit(damage);
            _attackAbleCount--;
        }
    }
}