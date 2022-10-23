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


        /*
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
        */

        public void displayImage()
        {
            StartCoroutine(showForSeconds(3));
        }

        IEnumerator showForSeconds(float time)
        {
            image.gameObject.SetActive(true);

            UI_ScreenConversation ui = UIManager.Instance.ShowNormalUI<UI_ScreenConversation>();
            ui.SetName("남모");
            ui.SetScriptTitle(Define.ScriptTitle.ForestCutScene1);

            float _time = 0;
            while (_time <= 2f)
            {
                image.color = new Color(1.0f, 1.0f, 1.0f, _time);
                _time += 0.01f;
                yield return null;
            }

            while (UIManager.Instance.getNormalUiCount() > 0) yield return null;


            ui = UIManager.Instance.ShowNormalUI<UI_ScreenConversation>();
            ui.SetName("준정");
            ui.SetScriptTitle(Define.ScriptTitle.ForestCutScene2);
            while (UIManager.Instance.getNormalUiCount() > 0) yield return null;

            ui = UIManager.Instance.ShowNormalUI<UI_ScreenConversation>();
            ui.SetName("남모");
            ui.SetScriptTitle(Define.ScriptTitle.ForestCutScene3);
            while (UIManager.Instance.getNormalUiCount() > 0) yield return null;

            ui = UIManager.Instance.ShowNormalUI<UI_ScreenConversation>();
            ui.SetName("준정");
            ui.SetScriptTitle(Define.ScriptTitle.ForestCutScene4);
            while (UIManager.Instance.getNormalUiCount() > 0) yield return null;

            ui = UIManager.Instance.ShowNormalUI<UI_ScreenConversation>();
            ui.SetName("남모");
            ui.SetScriptTitle(Define.ScriptTitle.ForestCutScene5);
            while (UIManager.Instance.getNormalUiCount() > 0) yield return null;

            _time = 0;
            while (_time <= 2f)
            {
                image.color = new Color(1.0f, 1.0f, 1.0f, 1 - _time);
                _time += 0.01f;
                yield return null;
            }
            InputController.Instance.GainControl();
            image.gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.CompareTag("Player"))
            {
                if (DataManager.Instance.GetForestCutSceneData() == 0)
                {

                    DataManager.Instance.ForestCutSceneDone();
                    DataManager.Instance.SaveGameData();
                    displayImage();
                }
            }
        }

    }


}

