using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Blind
{
    /// <summary>
    /// 씬의 트랜지션, 재시작 등을 관리하는 컨트롤러입니다.
    /// </summary>
    public class SceneController : Manager<SceneController>
    {
        public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }
        protected override void Awake()
        {
            base.Awake();
        }
        public void LoadScene(Define.Scene type)
        {
            SceneManager.LoadScene(GetSceneName(type));
        }

        string GetSceneName(Define.Scene type)
        {
            string name = System.Enum.GetName(typeof(Define.Scene), type);
            return name;
        }

        public void Clear()
        {
            CurrentScene.Clear();
        }

        public static void TransitionToScene(TransitionPoint transitionPoint)
        {
            Instance.StartCoroutine(Instance.Transition(transitionPoint.newSceneName, transitionPoint.transitionDestinationTag, transitionPoint.transitionType));
        }

        //여기에 씬 이동 전 인벤토리 세이브, 로딩, 게임 세이브 등 작업을 합니다.
        protected IEnumerator Transition(Define.Scene newSceneName, TransitionDestination.DestinationTag destinationTag, TransitionPoint.TransitionType transitionType = TransitionPoint.TransitionType.DifferentScene)
        {
            string sceneName = newSceneName.ToString();
            yield return SceneManager.LoadSceneAsync(sceneName);
            TransitionDestination entrance = GetDestination(destinationTag);
            SetEnteringLocation(entrance);
            yield return new WaitForSeconds(1);
        }

        protected TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag)
        {
            TransitionDestination[] entrances = FindObjectsOfType<TransitionDestination>();
            for (int i = 0; i < entrances.Length; i++)
            {
                if (entrances[i].destinationTag == destinationTag)
                    return entrances[i];
            }
            Debug.LogWarning("No Destination Found");
            return null;
        }

        // 캐릭터 이동
        protected void SetEnteringLocation(TransitionDestination entrance)
        {
            if (entrance == null)
            {
                Debug.LogWarning("entrance가 설정되지 않음");
                return;
            }
            if (entrance.transformingObject != null)
            {
                Transform entranceLocation = entrance.transform;
                Transform enteringTransform = entrance.transformingObject.transform;
                enteringTransform.position = entranceLocation.position;
                enteringTransform.rotation = entranceLocation.rotation;
            }
        }
    }
}

