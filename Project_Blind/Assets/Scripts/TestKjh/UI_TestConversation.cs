using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Data;

namespace Blind
{
    public class UI_TestConversation : UI_WorldSpace
    {
        int _defaultHeight = 2;
        int _defaultWidth = 10;
        int page = 0;
        string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        Dictionary<int, List<Data.Script>> conversations;
        Coroutine _showText;
        enum Texts
        {
            ConversationText,
            NPCNameText
        }
        enum Images
        {
            BackGroundImage,
            NextButtonImage,
            NPCNameImage
        }
        public override void Init()
        {
            base.Init();
            Bind<Text>(typeof(Texts));
            Bind<Image>(typeof(Images));
            Get<Image>((int)Images.NextButtonImage).gameObject.BindEvent(PushNextButton, Define.UIEvent.Click);
            Get<Image>((int)Images.BackGroundImage).gameObject.BindEvent(DragUI, Define.UIEvent.Drag);
        }
        protected override void Start()
        {
            conversations = ConversationScriptStorage.Instance.GetConversation(_title);
            Get<Text>((int)Texts.NPCNameText).text = Owner.name;
            ShowText(0);
        }
        void ShowText(int page)
        {
            int height = (conversations[ConversationScriptStorage.Instance.LanguageNumber][page].script.Length - 1) / _defaultWidth;
            if (height >= 10) height++;
            Get<Image>((int)Images.BackGroundImage).rectTransform.sizeDelta = new Vector2(_defaultWidth, _defaultHeight + height);
            _showText = StartCoroutine(CoShowTexts(page));
        }
        void PushNextButton(PointerEventData evt)
        {
            if (_showText != null)
            {
                StopCoroutine(_showText);
                _showText = null;
                string text = "";
                for (int i = 0; i < conversations[ConversationScriptStorage.Instance.LanguageNumber][page].script.Length; i++)
                {
                    if (i % 10 == 0 && i != 0) text += "\n";
                    text += conversations[ConversationScriptStorage.Instance.LanguageNumber][page].script[i];
                    Get<Text>((int)Texts.ConversationText).text = text;
                }
                return;
            }
            page++;
            if (page >= conversations[ConversationScriptStorage.Instance.LanguageNumber].Count)
            {
                UIManager.Instance.CloseWorldSpaceUI(this);
                Owner.GetComponent<ConversationTest>()._player.GetComponent<PlayerCharacter>().UnTalk();
                Owner.GetComponent<ConversationTest>()._state = Define.ObjectState.NonKeyDown;
                return;
            }
            ShowText(page);
        }
        void DragUI(PointerEventData evt)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos = new Vector3(pos.x, pos.y, 0);
            SetPosition(pos, Vector3.zero);
        }
        IEnumerator CoShowTexts(int page)
        {
            string text = "";
            for(int i = 0; i < conversations[ConversationScriptStorage.Instance.LanguageNumber][page].script.Length; i++)
            {
                if (i % 10 == 0 && i != 0) text += "\n";
                text += conversations[ConversationScriptStorage.Instance.LanguageNumber][page].script[i];
                Get<Text>((int)Texts.ConversationText).text = text;
                yield return new WaitForSeconds(0.1f);
            }
            _showText = null;
        }
    }
}

