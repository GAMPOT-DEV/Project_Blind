using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Blind
{
    public class UI_TestConversation : UI_WorldSpace
    {
        int _defaultHeight = 2;
        int _defaultWidth = 10;
        int page = 0;
        List<string> conversations;
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
            conversations = ConversationScriptStorage.Instance.GetConversation("Test");
            Bind<Text>(typeof(Texts));
            Bind<Image>(typeof(Images));
            Get<Image>((int)Images.NextButtonImage).gameObject.BindEvent(PushNextButton, Define.UIEvent.Click);
            Get<Image>((int)Images.BackGroundImage).gameObject.BindEvent(DragUI, Define.UIEvent.Drag);
            ShowText(0);
        }
        protected override void Start()
        {
            Get<Text>((int)Texts.NPCNameText).text = Owner.name;
        }
        void ShowText(int page)
        {
            int height = (conversations[page].Length - 1) / _defaultWidth;
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
                for (int i = 0; i < conversations[page].Length; i++)
                {
                    if (i % 10 == 0 && i != 0) text += "\n";
                    text += conversations[page][i];
                    Get<Text>((int)Texts.ConversationText).text = text;
                }
                return;
            }
            page++;
            if (page >= conversations.Count)
            {
                UIManager.Instance.CloseWorldSpaceUI(this);
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
            for(int i = 0; i < conversations[page].Length; i++)
            {
                if (i % 10 == 0 && i != 0) text += "\n";
                text += conversations[page][i];
                Get<Text>((int)Texts.ConversationText).text = text;
                yield return new WaitForSeconds(0.1f);
            }
            _showText = null;
        }
    }
}

