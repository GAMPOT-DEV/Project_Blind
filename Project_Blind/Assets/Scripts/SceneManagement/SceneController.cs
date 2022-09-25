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
        public bool isLoading;
        public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }
        protected override void Awake()
        {
            base.Awake();
            isLoading = false;
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
            if (!isLoading)
            {
                isLoading = true;
                yield return UI_ScreenFader.Instance.StartCoroutine(UI_ScreenFader.FadeScenOut());
                string sceneName = newSceneName.ToString();
                DataManager.Instance.PlayerCharacterDataValue.DestinationTag = destinationTag;
                yield return StartCoroutine(LoadingSceneController.LoadSceneProcess(sceneName,destinationTag));
                UI_ScreenFader.Instance.StartCoroutine(UI_ScreenFader.FadeSceneIn());
                isLoading = false;
            }
        }
        public static Vector3 SetDestination(TransitionDestination.DestinationTag destinationTag)
        {
            TransitionDestination[] entrances = FindObjectsOfType<TransitionDestination>();
            for (int i = 0; i < entrances.Length; i++)
            {
                if (entrances[i].destinationTag == destinationTag)
                {
                    var entrance = entrances[i];
                    Debug.Log(entrance);
                    return entrance.transform.position;
                }
            }
            return Vector3.one;
            ;
        }
    }
}

