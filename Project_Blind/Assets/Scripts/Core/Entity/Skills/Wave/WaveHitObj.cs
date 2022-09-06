using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

namespace Blind
{
    /// <summary>
    /// 파동에 맞은 오브젝트를 처리하는 클래스입니다.
    /// </summary>
    public class WaveHitObj : MonoBehaviour
    {
        private Coroutine _coroutine = null;
        private Renderer _renderer;
        private MaterialPropertyBlock _mpb;
        [SerializeField] private List<Material> meta = new List<Material>();
        private static readonly int ShowInShadow = Shader.PropertyToID("_ShowInShadow");

        public void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _mpb = new MaterialPropertyBlock();
        }

        public void GetHit()
        {
            if(_coroutine != null) StopCoroutine(_coroutine);
            _coroutine =  StartCoroutine(Glow());
        }


        private IEnumerator Glow()
        {
            SoundManager.Instance.Play("PureWaveSound1", Define.Sound.Effect);
            _mpb.SetFloat(ShowInShadow,1);
            _renderer.SetPropertyBlock(_mpb);
            yield return new WaitForSeconds(5f);
            _mpb.SetFloat(ShowInShadow,0);            
            _renderer.SetPropertyBlock(_mpb);
            
            _coroutine = null;
        }
    }
}