using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Blind
{
    public class BigTree : InteractionAble
    {
        public GameObject tree;
        private VideoPlayer videoPlayer;
        private PlayerCharacter player;
        private bool isWaveSense = false;
        private void Awake()
        {
            videoPlayer = tree.GetComponent<VideoPlayer>();
            //DataManager.Instance.ClearCaveData(); //동굴바위를 다시 생성시키고 싶다면 주석해제
        }

        public override void DoInteraction()
        {
            DataManager.Instance.CaveOpen();
            DataManager.Instance.SaveGameData();
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                player = collision.GetComponent<PlayerCharacter>();
                StartCoroutine(CheckWaveSpread());
            }
        }

        protected override void OnTriggerExit2D(Collider2D collision)
        {
        }

        IEnumerator CheckWaveSpread()
        {
            while (isWaveSense == false)
            {
                if (WaveSense.IsUsing)
                {
                    isWaveSense = true;
                    DoInteraction();
                    videoPlayer.Play();
                    yield return new WaitForSeconds(10.0f);
                    tree.SetActive(false);
                }
                else yield return null;
            }
        }
    }
}

