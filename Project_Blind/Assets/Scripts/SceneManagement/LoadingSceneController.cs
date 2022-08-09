using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Blind
{
    public class LoadingSceneController : MonoBehaviour
    {
        public static string nextScene;
        LoadingSceneController sceneController;

        private void Start()
        {
            //StartCoroutine(LoadSceneProcess());
        }

        public static void LoadScene(string sceneName)
        {
            nextScene = sceneName;
            //SceneManager.LoadScene("LoadingScene");
        }

        public static IEnumerator LoadSceneProcess(string nextScene)
        {
            yield return null;
            AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
            op.allowSceneActivation = false;
            float timer = 0.0f;
            while (!op.isDone)
            {
                yield return null;
                if (op.progress < 0.9f)
                {
                    //90프로 로딩까지는 계속 진행
                }
                else
                {
                    // 로딩이 90프로 이상 되면 1초간 추가 로딩을 함
                    timer += Time.unscaledDeltaTime;
                    if (timer >= 1.0f)
                    {
                        op.allowSceneActivation = true;
                        yield return UI_ScreenFader.Instance.StartCoroutine(UI_ScreenFader.FadeSceneIn());
                        Debug.Log("dfasd");
                        yield break;
                    }

                }
            }
        }
    }
}
