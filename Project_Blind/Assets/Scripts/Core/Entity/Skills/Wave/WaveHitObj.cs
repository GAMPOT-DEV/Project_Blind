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
        private SpriteRenderer _renderer;
        private Material _material;
        public void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _material = _renderer.material;
        }

        public void GetHit()
        {
            if(_coroutine != null) StopCoroutine(_coroutine);
            _coroutine =  StartCoroutine(Glow());
        }


        private IEnumerator Glow()
        {
            SoundManager.Instance.Play("PureWaveSound1", Define.Sound.Effect);
            _material.SetFloat("_ShowInShadow",1);            
            yield return new WaitForSeconds(5f);
            _material.SetFloat("_ShowInShadow",0);            
            _coroutine = null;
        }
    }
}