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
        public void Awake()
        {
            GetComponent<Light2D>().intensity = 0;
        }

        public void GetHit()
        {
            StartCoroutine(Glow());
        }

        private IEnumerator Glow()
        {
            GetComponent<Light2D>().intensity = 1;
            yield return new WaitForSeconds(5f);
            GetComponent<Light2D>().intensity = 0;
        }
    }
}