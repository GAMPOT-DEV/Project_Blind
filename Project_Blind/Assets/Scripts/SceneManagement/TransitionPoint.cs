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
        [SerializeField] Define.Scene NextSceneName;
        [Tooltip("이동하고 싶은 게임오브젝트를 인스펙터창에서 집어넣으면 됩니다. ex)메인캐릭터")]
        public GameObject transitioningGameObject;


        private void Awake()
        {
                   }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == transitioningGameObject)
            {
                TransitionInternal();
            }
        }

        protected void TransitionInternal()
        {
            //씬을 옮기기 전에 가져가야 할 데이터가 있는지 확인합니다
            //if(inventoryCheck)
            //..

            if (transitionType == TransitionType.SameScene)
            {
                //같은 씬의 다른 포인트로 이동하는 경우 아래 위치로 이동합니다.
                //..
            }

            else
            {
                //다른 씬으로 이동할 경우입니다.
                SceneController.Instance.LoadScene(NextSceneName); //여기서 SceneController 선언 없이 할 수 없을까..? SceneController.LoadScene(Define.Scene.황현택_dest);
            }
        }
    }

}
