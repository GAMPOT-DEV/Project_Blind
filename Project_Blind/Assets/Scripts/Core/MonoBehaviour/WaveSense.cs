using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  Blind
{
    /// <summary>
    /// 파동의 로직을 처리하는 클래스입니다.
    /// </summary>
    public class WaveSense : MonoBehaviour
    {
        private float _maxRadious = 20f;
        private float _spreadSpeed = 0.5f;
        private CircleCollider2D _collider2D;
        private Coroutine _coroutine = null;

        private void Awake()
        {
            _collider2D = GetComponent<CircleCollider2D>();
            _collider2D.enabled = false;
        }

        public void StartSpread()
        {
            if(_coroutine != null) return;
            _coroutine = StartCoroutine(Spread());
        }

        private IEnumerator Spread()
        {
            _collider2D.enabled = true;
            while (_collider2D.radius < _maxRadious)
            {
                _collider2D.radius += _spreadSpeed;
                yield return new WaitForSeconds(.05f);
            }
            _collider2D.radius = 0;
            _collider2D.enabled = false;
            _coroutine = null;
            ResourceManager.Instance.Destroy(this.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            var waveHitObj = col.gameObject.GetComponent<WaveHitObj>();
            if (waveHitObj != null)
            {
                waveHitObj.GetHit();
            }
        }

    }
}
