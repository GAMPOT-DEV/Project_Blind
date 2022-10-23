using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
namespace Blind
{
    public class BossWaveSense : MonoBehaviour
    {
        [SerializeField] private float _maxRadious = 20f;
        [SerializeField] private AnimationCurve spreadSpeed;
        [SerializeField] private AnimationCurve fadeAnimation;
        [SerializeField] private float fadeTime = 0.5f;
        private float _curTime;
        private CircleCollider2D _collider2D;
        private Light2D _light;
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
            _light = GetComponent<Light2D>();
            GameManager.Instance.Player.isBossWaveCheck = false;
            _radius = 0;
        }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            var waveHitObj = col.gameObject.GetComponent<WaveHitObj>();
            if (waveHitObj != null)
            {
                waveHitObj.GetHit();
            }
            else if (col.tag.Equals("Player")  && col.gameObject.GetComponent<PlayerCharacter>().isBossWaveCheck)
            {
                col.gameObject.GetComponent<PlayerCharacter>().HitWithKnockBack(new AttackInfo(3,Facing.Right));
            }
            else if (col.tag.Equals("Player") && !col.gameObject.GetComponent<PlayerCharacter>().isBossWaveCheck)
            {
                col.gameObject.GetComponent<PlayerCharacter>().isBossWaveCheck = true;
            }
        }

        public void StartSpread()
        {
            if(_coroutine != null) return;
            _coroutine = StartCoroutine(Spread());
            _isUsing = true;
        }

        private IEnumerator Spread()
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
            _curTime = 0;
            var maxLight = _light.intensity;
            while (_light.intensity > 0)
            {
                _curTime += Time.deltaTime;
                _light.intensity = fadeAnimation.Evaluate(_curTime / fadeTime) * maxLight;
                yield return null;
            }

            _light.intensity = maxLight;
            _radius = 0;
            _collider2D.enabled = false;
            _coroutine = null;
            _isUsing = false;
            ResourceManager.Instance.Destroy(this.gameObject);
        }
    }
}