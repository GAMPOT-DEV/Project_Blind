using System;
using System.Collections;
using UnityEngine;

namespace Blind
{
    public class EndingScene: MonoBehaviour
    {
        [SerializeField] private GameObject credit;
        [SerializeField] private float end;
        [SerializeField] private TransitionPoint tp;
        private bool isCheck = false;
        public void Awake()
        {
            SoundManager.Instance.Play("BGM/숲",Define.Sound.Bgm);
        }

        public void FixedUpdate()
        {
            if(isCheck) return;
            
            if (credit.transform.position.y >= end)
            {
                StartCoroutine(End());
                isCheck = true;
            }
        }

        public IEnumerator End()
        {
            yield return new WaitForSeconds(5f);
            SoundManager.Instance.StopBGM();
            tp.TransitionInternal();
        }
    }
}