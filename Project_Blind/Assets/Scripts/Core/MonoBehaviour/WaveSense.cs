using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Blind
{
    /// <summary>
    /// 파동의 로직을 처리하는 클래스입니다.
    /// </summary>
    public class WaveSense : MonoBehaviour
    {
        private float _maxRadious = 20f;
        [SerializeField] private float period = 1.5f;
        private float _curTime;
        private CircleCollider2D _collider2D;
        private Coroutine _coroutine = null;
        private float _radius;

        private static bool _isUsing;
        public static bool IsUsing
        {
            get { return _isUsing; }
        }
        private void Awake()
        {
            _collider2D = GetComponent<CircleCollider2D>();
            _collider2D.enabled = false;
            _radius = 0;
        }

        private void Update()
        {
        }

        public void StartSpread(AnimationCurve spreadSpeed)
        {
            if(_coroutine != null) return;
            _coroutine = StartCoroutine(Spread(spreadSpeed));
            _isUsing = true;
        }

        private IEnumerator Spread(AnimationCurve spreadSpeed)
        {
            _curTime = 0;
            _collider2D.enabled = true;
            while (_radius<_maxRadious)
            {
                _curTime += Time.deltaTime;
                _radius = spreadSpeed.Evaluate(_curTime) * _maxRadious;
                transform.localScale = new Vector3(_radius, _radius, 0);
                yield return null;
            }
            _radius = 0;
            _collider2D.enabled = false;
            _coroutine = null;
            _isUsing = false;
            ResourceManager.Instance.Destroy(this.gameObject);
        }
    }
}
