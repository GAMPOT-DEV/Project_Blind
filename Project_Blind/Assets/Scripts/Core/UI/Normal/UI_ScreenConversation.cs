using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Blind
{
    public class UI_ScreenConversation : UI_Base
    {
        enum Images
        {
            Image_Button
        }
        enum Texts
        {
            Text_Name,
            Text_Script
        }

        int page = 0;
        string _titleStr;
        string _name = "";

        public Action EndEvent = null;
        public bool StopMove = true;

        Coroutine _showText;

        Dictionary<int, List<Data.Script>> conversations;
        public override void Init()
        {
            Bind<Image>(typeof(Images));
            Bind<Text>(typeof(Texts));
            Get<Image>((int)Images.Image_Button).gameObject.BindEvent(PushNextButton, Define.UIEvent.Click);

            UIManager.Instance.KeyInputEvents -= HandleUIKeyInput;
            UIManager.Instance.KeyInputEvents += HandleUIKeyInput;

            if (StopMove)
            {
                PlayerCharacter player = FindObjectOfType<PlayerCharacter>();
                if (player != null) player.Talk();
            }
        }
        protected override void Start()
        {
            conversations = ConversationScriptStorage.Instance.GetConversation(_titleStr);
            Get<Text>((int)Texts.Text_Name).text = _name;
            ShowText(0);
        }
        public void SetScriptTitle(Define.ScriptTitle title)
        {
            _titleStr = Enum.GetName(typeof(Define.ScriptTitle), title);
        }
        public void SetName(string name)
        {
            _name = name;
        }
        void ShowText(int page)
        {
            _showText = StartCoroutine(CoShowTexts(page));
        }
        void PushNextButton()
        {
            if (_showText != null)
            {
                StopCoroutine(_showText);
                _showText = null;
                string text = "";
                for (int i = 0; i < conversations[ConversationScriptStorage.Instance.LanguageNumber][page].script.Length; i++)
                {
                    //if (i % 10 == 0 && i != 0) text += "\n";
                    text += conversations[ConversationScriptStorage.Instance.LanguageNumber][page].script[i];
                    Get<Text>((int)Texts.Text_Script).text = text;
                }
                return;
            }
            page++;
            if (page >= conversations[ConversationScriptStorage.Instance.LanguageNumber].Count)
            {
                UIManager.Instance.CloseNormalUI(this);
                if (StopMove)
                {
                    PlayerCharacter player = FindObjectOfType<PlayerCharacter>();
                    if (player != null) player.UnTalk();
                    UIManager.Instance.KeyInputEvents -= HandleUIKeyInput;
                }
                if (EndEvent != null) EndEvent.Invoke();
                EndEvent = null;
                return;
            }
            ShowText(page);
        }
        IEnumerator CoShowTexts(int page)
        {
            string text = "";
            for (int i = 0; i < conversations[ConversationScriptStorage.Instance.LanguageNumber][page].script.Length; i++)
            {
                if (i % 10 == 0 && i != 0)
                {
                    //text += "\n";
                }
                text += conversations[ConversationScriptStorage.Instance.LanguageNumber][page].script[i];
                Get<Text>((int)Texts.Text_Script).text = text;
                yield return new WaitForSeconds(0.1f);
            }
            _showText = null;
        }
        #region Update
        private void HandleUIKeyInput()
        {
            if (!Input.anyKey)
                return;

            if (_uiNum != UIManager.Instance.UINum)
                return;

            if (Input.GetKeyDown(KeyCode.Return))
            {
                PushNextButton();
                return;
            }
        }
        #endregion
    }
}

