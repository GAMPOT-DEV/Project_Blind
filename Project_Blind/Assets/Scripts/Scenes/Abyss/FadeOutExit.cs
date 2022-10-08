using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Blind.Abyss
{
    public class FadeOutExit : Stage
    {
        public GameObject ExitPoint;
        private PlayerCharacter Player;
        public Image panel;
        public Transform exit;
        protected override void Awake()
        {
            ExitPoint.GetComponent<ExitPoint>().stage = this;
        }

        private void Start()
        {
            Player = GameManager.Instance.Player;
        }

        public void StartFadeOut()
        {
            StartCoroutine(FadeOut());
        }

        IEnumerator FadeOut()
        {
            float count = 0;
            while(count<1.0f)
            {
                count += 0.01f;
                yield return new WaitForSeconds(0.01f);
                panel.color = new Color(1.0f,1.0f ,1.0f , count);
            }
            Player.transform.position = exit.position;
        }
    }

}
