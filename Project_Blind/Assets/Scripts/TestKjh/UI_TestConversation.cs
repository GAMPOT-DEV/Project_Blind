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

        int _currHeight;
        int _maxHeight;

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
            // 스크립트를 띄우는 주인의 이름 출력
            Get<Text>((int)Texts.NPCNameText).text = Owner.name;

            // 맨 처음에 0번 페이지의 텍스트 출력
            ShowText(0);
        }
        void ShowText(int page)
        {
            // 글자수에 따라 ui의 높이 변경
            _maxHeight = (conversations[ConversationScriptStorage.Instance.LanguageNumber][page].script.Length - 1) / _defaultWidth;
            if (_maxHeight >= 10) _maxHeight++;
            _currHeight = 0;
            Get<Image>((int)Images.BackGroundImage).rectTransform.sizeDelta = new Vector2(_defaultWidth, _defaultHeight + _currHeight);

            // 한글자씩 출력하도록 하는 코루틴 시작
            _showText = StartCoroutine(CoShowTexts(page));
        }
        void PushNextButton()
        {
            // _showText 코루틴이 아직 null이 아니면 CoShowTexts가 실행중이기 때문에 넘기면 안되고
            // 텍스트를 즉시 다 띄우도록 하고 함수 종료
            if (_showText != null)
            {
                StopCoroutine(_showText);
                _showText = null;
                string text = "";
                _currHeight = _maxHeight;
                Get<Image>((int)Images.BackGroundImage).rectTransform.sizeDelta = new Vector2(_defaultWidth, _defaultHeight + _currHeight);
                for (int i = 0; i < conversations[ConversationScriptStorage.Instance.LanguageNumber][page].script.Length; i++)
                {
                    if (i % 10 == 0 && i != 0) text += "\n";
                    text += conversations[ConversationScriptStorage.Instance.LanguageNumber][page].script[i];
                    Get<Text>((int)Texts.ConversationText).text = text;
                }
                return;
            }
            // 다음 버튼을 눌렀고, _showText 코루틴이 null이기 때문에 다음 페이지로 넘어가도 된다.
            // 페이지를 1 늘려주고, 만약 페이지가 전체 스크립트의 페이지를 넘어가면 ui를 닫아주고
            // 플레이어를 움직일 수 있는 상태로 만들어주고 함수를 종료한다.
            page++;
            if (page >= conversations[ConversationScriptStorage.Instance.LanguageNumber].Count)
            {
                UIManager.Instance.CloseWorldSpaceUI(this);
                Owner.GetComponent<ConversationTest>()._player.GetComponent<PlayerCharacter>().UnTalk();
                Owner.GetComponent<ConversationTest>()._state = Define.ObjectState.NonKeyDown;
                return;
            }

            // 여기까지 왔으면 _showText코루틴이 실행중인것도 아니고, 다음 페이지가 유효한 페이지이다.
            // 다음 페이지릐 텍스트를 ui에 띄워주는 ShowText함수를 실행한다.
            ShowText(page);
        }
        void DragUI()
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos = new Vector3(pos.x, pos.y, 0);
            SetPosition(pos, Vector3.zero);
        }
        IEnumerator CoShowTexts(int page)
        {
            // 텍스트를 한줄에 전부 띄울수는 없으니 줄바꿈을 적용할 텍스트를 새로 만듬
            string text = "";
            for (int i = 0; i < conversations[ConversationScriptStorage.Instance.LanguageNumber][page].script.Length; i++)
            {
                // 글자수 10개 넘어가면 줄바꿈
                if (i % 10 == 0 && i != 0)
                {
                    _currHeight++;
                    Get<Image>((int)Images.BackGroundImage).rectTransform.sizeDelta = new Vector2(_defaultWidth, _defaultHeight + _currHeight);
                    text += "\n";
                }
                // 한글자씩 text에 추가
                text += conversations[ConversationScriptStorage.Instance.LanguageNumber][page].script[i];
                // ui에 텍스트를 띄움
                Get<Text>((int)Texts.ConversationText).text = text;
                // 0.1초 대기
                yield return new WaitForSeconds(0.1f);
            }
            // _showText 코루틴 null로 설정
            _showText = null;
        }
    }
}

