using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class TransitionPoint : MonoBehaviour
    {
        public enum TransitionType
        {
            SameScene, DifferentScene, DifferentNonGameplayScene,
        }

        [Tooltip("인스펙터 창에서 이동 타입을 선택해주면 됩니다.")]
        public TransitionType transitionType;
        [Tooltip("이동할 다음 씬을 선택하면 됩니다.")]
        public Define.Scene newSceneName;
        /*
        [Tooltip("이동하고 싶은 게임오브젝트를 인스펙터창에서 집어넣으면 됩니다. ex)메인캐릭터")]
        public GameObject transitioningGameObject; */
        [Tooltip("도착지점의 태그")]
        public TransitionDestination.DestinationTag transitionDestinationTag;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag.Equals("Player"))
            {
                TransitionInternal();
            }
        }

        public IEnumerator LodingTranition()
        {
            bool isLoading = false;
            if (!isLoading)
            {
                isLoading = true;
                yield return StartCoroutine(UI_ScreenFader.FadeScenOut());
                yield return StartCoroutine(LoadingSceneController.LoadSceneProcess("LoadingScene", true));
                yield return StartCoroutine(UI_ScreenFader.FadeSceneIn());
                isLoading = false;
            }
        }
        public void TransitionInternal()
        {
            //씬을 옮기기 전에 가져가야 할 데이터가 있는지 확인합니다
            //if(inventorysssheck)
            //..

            if (transitionType == TransitionType.SameScene)
            {
                //같은 씬의 다른 포인트로 이동하는 경우 아래 위치로 이동합니다.
                //..
            }

            else
            {
                //다른 씬으로 이동할 경우입니다.
                if (GameManager.IsExist())
                {
                    var player = GameManager.Instance.Player;
                    Debug.Log(player);
                    if (player != null)
                        DataManager.Instance.PlayerCharacterDataValue =
                            new PlayerCharacterData(player.Hp, player.CurrentWaveGauge);
                }
                LodingHub.Instance.StartNextScene(this);
                StartCoroutine(LodingTranition());
            }
        }
    }

}
