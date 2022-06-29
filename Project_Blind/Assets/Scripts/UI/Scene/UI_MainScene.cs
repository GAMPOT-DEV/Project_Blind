using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Blind
{
    public class UI_MainScene : UI_Scene
    {
        enum Buttons
        {
            Button_Start,
            Button_Option,
            Button_Exit,
        }
        enum Texts
        {
            Text_Start,
            Text_Option,
            Text_Exit,
        }
        public override void Init()
        {
            base.Init();
            Debug.Log("Main Scene");
            Bind<Button>(typeof(Buttons));
            Bind<Text>(typeof(Texts));
            Get<Text>((int)Texts.Text_Start).text = "Start";
            Get<Text>((int)Texts.Text_Option).text = "Option";
            Get<Text>((int)Texts.Text_Exit).text = "Exit";
            Get<Button>((int)Buttons.Button_Start).gameObject.BindEvent(PushStartButton, Define.UIEvent.Click);
            Get<Button>((int)Buttons.Button_Option).gameObject.BindEvent(PushOptionButton, Define.UIEvent.Click);
            Get<Button>((int)Buttons.Button_Exit).gameObject.BindEvent(PushExitButton, Define.UIEvent.Click);
        }
        private void PushStartButton()
        {
            Debug.Log("Push Start");
        }
        private void PushOptionButton()
        {
            Debug.Log("Push Option");
        }
        private void PushExitButton()
        {
            Debug.Log("Push Exit");
        }
    }
}

