using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Blind
{
    public class CutSceneImage : MonoBehaviour
    {
        public Image image;
        public Color color;
        private bool flag = false;

        public void Start()
        {
            image.gameObject.SetActive(false);
        }


        IEnumerator waitForInput()
        {
            while (flag == false)
            {
                if (Input.GetKeyDown(KeyCode.N))
                {
                    displayImage();
                    flag = true;
                    break;
                }
                yield return null;
            }
        }

        public void displayImage()
        {
            StartCoroutine(showForSeconds(3));
        }

        IEnumerator showForSeconds(float time)
        {
            image.gameObject.SetActive(true);
            float _time = 0;
            while (_time <= 2f)
            {
                image.color = new Color(1.0f, 1.0f, 1.0f, _time);
                _time += 0.01f;
                yield return null;
            }
            _time = 0;
            while (_time <= time)
            {
                _time += 0.01f;
                yield return null;
            }
            _time = 0;
            while (_time <= 2f)
            {
                image.color = new Color(1.0f, 1.0f, 1.0f, 1 - _time);
                _time += 0.01f;
                yield return null;
            }
            image.gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.CompareTag("Player"))
            {
                StartCoroutine(waitForInput());
            }
        }

    }


}

