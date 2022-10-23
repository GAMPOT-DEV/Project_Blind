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
        private Vector2 movePosition;
        [FormerlySerializedAs("_parent")] public BossPattern3 Parent;
        private bool isPlay = false;
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

        public void FixedUpdate()
        {
            if (!isPlay) return;

            transform.position = Vector2.MoveTowards(transform.position, movePosition, 0.2f);

            if (transform.position.y == movePosition.y)
            {
                transform.position = _originPos;
                gameObject.SetActive(false);
                isPlay = false;
            }
        }

        public Coroutine Play(Vector2 movePosiotion)
        {
            movePosition = movePosiotion;
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
            isPlay = true;
            yield return new WaitForSeconds(3f);
        }
    }
}