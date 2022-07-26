using Blind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// TEST CLASS
namespace Blind
{
    public class UI_TestSceneUI : UI_Scene
    {
        int _clickCnt = 0;
        int _dragCnt = 0;
        enum Texts
        {
            TestText
        }
        enum Buttons
        {
            TestButton
        }
        public override void Init()
        {
            Bind<Text>(typeof(Texts));
            Bind<Button>(typeof(Buttons));
            Get<Button>((int)Buttons.TestButton).gameObject.BindEvent(OnClick);
            Get<Button>((int)Buttons.TestButton).gameObject.BindEvent(OnDrag, Define.UIEvent.Drag);
            Get<Button>((int)Buttons.TestButton).gameObject.BindEvent(OnEndDrag, Define.UIEvent.EndDrag);
        }
        void OnClick()
        {
            _clickCnt++;
            RefreshUI(Define.UIEvent.Click);
        }
        void OnDrag()
        {
            _dragCnt++;
            RefreshUI(Define.UIEvent.Drag);
        }
        void OnEndDrag()
        {
            _dragCnt = 0;
            RefreshUI(Define.UIEvent.EndDrag);
        }
        void RefreshUI(Define.UIEvent evt)
        {
            if (evt == Define.UIEvent.Click)
            {
                Get<Text>((int)Texts.TestText).text = "Click : " + _clickCnt.ToString();
            }
            else if (evt == Define.UIEvent.Drag)
            {
                Get<Text>((int)Texts.TestText).text = "Drag : " + _dragCnt.ToString();
            }
            else if (evt == Define.UIEvent.EndDrag)
            {
                Get<Text>((int)Texts.TestText).text = "EndDrag";
            }
        }
    }

}
