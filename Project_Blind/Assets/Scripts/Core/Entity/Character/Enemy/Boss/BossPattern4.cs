using System;
using System.Collections;
using UnityEngine;

namespace Blind
{
    /// <summary>
    /// 물기 패턴
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public class BossPattern4: BossAttackPattern<FirstBossEnemy>
    {
        private BoxCollider2D _collider;
        public void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
        }

        public override void AttackPattern()
        {
            Debug.Log("pattern4");
            StartCoroutine(attackPattern());
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            
        }

        private IEnumerator attackPattern()
        {
            
            yield return null;
        }
    }
}