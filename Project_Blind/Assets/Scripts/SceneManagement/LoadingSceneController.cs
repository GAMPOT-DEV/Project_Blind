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

        public static IEnumerator LoadSceneProcess(string nextScene,TransitionDestination.DestinationTag destinationTag)
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
                    //90���� �ε������� ��� ����
                }
                else
                {
                    // �ε��� 90���� �̻� �Ǹ� 1�ʰ� �߰� �ε��� ��
                    timer += Time.unscaledDeltaTime;
                    if (timer >= 1.0f)
                    {
                        op.allowSceneActivation = true;
                        yield break;
                    }

                }
            }
        }

    }
}
