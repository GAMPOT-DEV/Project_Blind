using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Blind
{
    public class UI_MainScene : UI_Scene
    {
        const int BUTTON_COUNT = (int)Buttons.Button_Exit + 1;
        private Action[] _actions = new Action[BUTTON_COUNT];
        private int _currCursor;
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
        enum Images
        {
            Image_Cursor,
        }
        public override void Init()
        {
            base.Init();
            _uiNum = UIManager.Instance.UINum;
            Debug.Log("Main Scene");
            Bind<Button>(typeof(Buttons));
            Bind<Text>(typeof(Texts));
            Bind<Image>(typeof(Images));
            Get<Text>((int)Texts.Text_Start).text = "Start";
            Get<Text>((int)Texts.Text_Option).text = "Option";
            Get<Text>((int)Texts.Text_Exit).text = "Exit";
            Get<Button>((int)Buttons.Button_Start).gameObject.BindEvent(PushStartButton, Define.UIEvent.Click);
            Get<Button>((int)Buttons.Button_Option).gameObject.BindEvent(PushOptionButton, Define.UIEvent.Click);
            Get<Button>((int)Buttons.Button_Exit).gameObject.BindEvent(PushExitButton, Define.UIEvent.Click);

            _currCursor = 0;
            Get<Image>((int)Images.Image_Cursor).transform.position = Get<Button>(_currCursor).transform.position;
            _actions[0] += PushStartButton;
            _actions[1] += PushOptionButton;
            _actions[2] += PushExitButton;
        }
        private void Update()
        {
            HandleKeyInput();
        }
        private void HandleKeyInput()
        {
            if (!Input.anyKey)
                return;

            if (_uiNum != UIManager.Instance.UINum)
            {
                Debug.Log(_uiNum);
                Debug.Log(UIManager.Instance.UINum);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("Enter");
                _actions[_currCursor].Invoke();
                return;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                _currCursor = (_currCursor + 1) % BUTTON_COUNT;
                Get<Image>((int)Images.Image_Cursor).transform.position = Get<Button>(_currCursor).transform.position;
                return;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                _currCursor = (_currCursor - 1 + BUTTON_COUNT) % BUTTON_COUNT;
                Get<Image>((int)Images.Image_Cursor).transform.position = Get<Button>(_currCursor).transform.position;
                return;
            }
        }
        private void PushStartButton()
        {
            Debug.Log("Push Start");
        }
        private void PushOptionButton()
        {
            Debug.Log("Push Option");
            UIManager.Instance.ShowNormalUI<UI_Setting>();
        }
        private void PushExitButton()
        {
            Debug.Log("Push Exit");
            Application.Quit();
        }
    }
}

