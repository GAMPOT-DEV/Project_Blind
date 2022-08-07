using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Blind
{
    [RequireComponent(typeof(Light2D))]
    public class GlowStone : MonoBehaviour
    {
        [SerializeField] private AnimationCurve spreadAnimation;
        [SerializeField] private float maxSize = 30f;

        private Light2D _light2D;
        private bool _isBright = false;

        private void Awake()
        {
            _light2D = GetComponent<Light2D>();
        }

        /// <summary>
        /// 테스트
        /// </summary>
        private void Start()
        {
            Bright();
        }

        /// <summary>
        /// 이 함수 호출하면 켜집니다.
        /// </summary>
        public void Bright()
        {
            if (_isBright) return;
            StartCoroutine(_Bright());
            _isBright = true;
        }

        public IEnumerator _Bright()
        {
            var curTime = 0f;
            var radius = 0f;
            while (radius < maxSize)
            {
                curTime += Time.deltaTime;
                radius += spreadAnimation.Evaluate(curTime) * maxSize;
                _light2D.pointLightOuterRadius = radius;
                yield return null;
            }
        }
    }
}