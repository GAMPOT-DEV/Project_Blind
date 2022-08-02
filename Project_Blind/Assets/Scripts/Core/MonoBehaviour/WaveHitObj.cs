using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Blind
{
    /// <summary>
    /// 파동에 맞은 오브젝트를 처리하는 클래스입니다.
    /// </summary>
    public class WaveHitObj : MonoBehaviour
    {
        private Coroutine _coroutine = null;
        public void Awake()
        {
            GetComponent<Light2D>().intensity = 0;
        }

        public void GetHit()
        {
            if(_coroutine != null) StopCoroutine(_coroutine);
            _coroutine =  StartCoroutine(Glow());
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.GetComponent<WaveSense>() != null)
            {
                GetHit();
            }
        }

        private IEnumerator Glow()
        {
            SoundManager.Instance.Play("PureWaveSound", Define.Sound.Effect);

            GetComponent<Light2D>().intensity = 1;
            if (GetComponent<SpriteRenderer>() != null)
            yield return new WaitForSeconds(5f);
            if (GetComponent<SpriteRenderer>() != null)
            GetComponent<Light2D>().intensity = 0;
            _coroutine = null;
        }
    }
}