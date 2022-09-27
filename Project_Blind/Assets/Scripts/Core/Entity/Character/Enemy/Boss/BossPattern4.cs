using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
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
        private Chin _upperChin;
        private Chin _lowerChin;
        private int _attackAbleCount = 1;

        private void Awake()
        {
            _upperChin = transform.GetChild(0).GetComponent<Chin>();
            _lowerChin = transform.GetChild(1).GetComponent<Chin>();
            _upperChin.Parent = this;
            _lowerChin.Parent = this;
        }

        public override Coroutine AttackPattern()
        {
            transform.position = GameManager.Instance.Player.transform.position;
            _upperChin.gameObject.SetActive(true);
            _lowerChin.gameObject.SetActive(true);
            _upperChin.Play();
            return _lowerChin.Play();
        }

        public void Attack()
        {
            if (_attackAbleCount == 0) return;
            GameManager.Instance.Player.Hit(damage);
            _attackAbleCount--;
        }
    }
}