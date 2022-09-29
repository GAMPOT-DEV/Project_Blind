using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Blind
{
    public class UI_CutScene : MonoBehaviour
    {
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private GameObject black;
        bool _stop1 = true;
        bool _stop2 = false;
        bool _stop3 = false;
        bool _stop4 = false;
        bool _stop5 = false;
        bool _stop6 = false;

        bool finish = false;
        TransitionPoint _transition;
        private void Start()
        {
            black.SetActive(false);
            _transition = FindObjectOfType<TransitionPoint>();
        }
        private void Update()
        {
            if(_stop1 && videoPlayer.time >= 5f)
            {
                videoPlayer.Pause();
                UI_ScreenConversation ui = UIManager.Instance.ShowNormalUI<UI_ScreenConversation>();
                ui.SetName("준정");
                ui.SetScriptTitle(Define.ScriptTitle.CutScene1);
                ui.EndEvent += Play;
                _stop1 = false;
                _stop2 = true;
            }
            if (_stop2 && videoPlayer.time >= 7.5f)
            {
                videoPlayer.Pause();
                UI_ScreenConversation ui = UIManager.Instance.ShowNormalUI<UI_ScreenConversation>();
                ui.SetName("남모");
                ui.SetScriptTitle(Define.ScriptTitle.CutScene2);
                ui.EndEvent += Play;
                _stop2 = false;
                _stop3 = true;
            }
            if (_stop3 && videoPlayer.time >= 10f)
            {
                videoPlayer.Pause();
                UI_ScreenConversation ui = UIManager.Instance.ShowNormalUI<UI_ScreenConversation>();
                ui.SetName("준정");
                ui.SetScriptTitle(Define.ScriptTitle.CutScene3);
                ui.EndEvent += Play;
                _stop3 = false;
                _stop4 = true;
            }
            if (_stop4 && videoPlayer.time >= 12.5f)
            {
                videoPlayer.Pause();
                UI_ScreenConversation ui = UIManager.Instance.ShowNormalUI<UI_ScreenConversation>();
                ui.SetName("남모");
                ui.SetScriptTitle(Define.ScriptTitle.CutScene4);
                ui.EndEvent += Play;
                _stop4 = false;
                _stop5 = true;
            }
            if (_stop5 && videoPlayer.time >= 14.5f)
            {
                videoPlayer.Pause();
                UI_ScreenConversation ui = UIManager.Instance.ShowNormalUI<UI_ScreenConversation>();
                ui.SetName("준정");
                ui.SetScriptTitle(Define.ScriptTitle.CutScene5);
                ui.EndEvent += Play;
                _stop5 = false;
                _stop6 = true;
            }
            if (_stop6 && videoPlayer.time >= 17f)
            {
                videoPlayer.Pause();
                UI_ScreenConversation ui = UIManager.Instance.ShowNormalUI<UI_ScreenConversation>();
                ui.SetName("준정");
                ui.SetScriptTitle(Define.ScriptTitle.CutScene6);
                ui.EndEvent += Play;
                _stop6 = false;
            }
            if (videoPlayer.clip.length - videoPlayer.time <= 0.2f && finish == false)
            {
                // 암전
                finish = true;
                black.SetActive(true);
                StartCoroutine(CoDisplayLastScript());
            }
        }
        private void Play()
        {
            videoPlayer.Play();
        }
        private void GoNextScene()
        {
            UIManager.Instance.Clear();
            _transition.TransitionInternal();
        }
        IEnumerator CoDisplayLastScript()
        {
            SoundManager.Instance.StopBGM();
            yield return new WaitForSeconds(0.5f);
            UI_ScreenConversation ui = UIManager.Instance.ShowNormalUI<UI_ScreenConversation>();
            ui.SetScriptTitle(Define.ScriptTitle.CutScene7);
            ui.EndEvent += GoNextScene;
        }
    }
}

