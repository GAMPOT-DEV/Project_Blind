using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Blind
{
    public class Chin : MonoBehaviour
    {
        [SerializeField] private ChinDir dir;
        private Vector3  _originPos;
        [FormerlySerializedAs("_parent")] public BossPattern4 Parent;
        public enum ChinDir
        {
            Upper = 1,
            Lower = -1
        }

        public void Awake()
        {
            gameObject.SetActive(false);
            _originPos = transform.position;
        }

        public Coroutine Play()
        {
            return StartCoroutine(play());
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                Parent.Attack();
            }
        }

        private IEnumerator play()
        {
            var curTime = 0f;
            while (curTime < 1)
            {
                curTime += Time.deltaTime;
                transform.position += Vector3.down * (float) dir * 0.1f;
                yield return null;
            }
            transform.position = _originPos;
            gameObject.SetActive(false);
        }
    }
}