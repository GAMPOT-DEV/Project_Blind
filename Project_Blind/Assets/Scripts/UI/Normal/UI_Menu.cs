using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Blind
{
    public class UI_Menu : UI_Base
    {
        #region Enums
        enum Images
        {
            Image_Bag,
            Image_Talisman,
            Image_Setting,
            Image_Main,

            Image_Close,
            Image_Cursor,
        }
        #endregion
        const int MENU_SIZE = (int)Images.Image_Main + 1;
        private Action[] _actions = new Action[MENU_SIZE];

        private int _currCursor;


        TransitionPoint _transition;
        public override void Init()
        {
            Bind<Image>(typeof(Images));
            InitEvents();
            Time.timeScale = 0;

            _transition = FindObjectOfType<TransitionPoint>();
        }
        private void InitEvents()
        {
            Get<Image>((int)Images.Image_Bag).gameObject.BindEvent(PushBagButton, Define.UIEvent.Click);
            Get<Image>((int)Images.Image_Talisman).gameObject.BindEvent(PushTalismanButton, Define.UIEvent.Click);
            Get<Image>((int)Images.Image_Setting).gameObject.BindEvent(PushSettingButton, Define.UIEvent.Click);
            Get<Image>((int)Images.Image_Main).gameObject.BindEvent(PushMainButton, Define.UIEvent.Click);
            Get<Image>((int)Images.Image_Close).gameObject.BindEvent(PushCloseButton, Define.UIEvent.Click);

            _actions[(int)Images.Image_Bag] += PushBagButton;
            _actions[(int)Images.Image_Talisman] += PushTalismanButton;
            _actions[(int)Images.Image_Setting] += PushSettingButton;
            _actions[(int)Images.Image_Main] += PushMainButton;

            _currCursor = (int)Images.Image_Bag;
            Get<Image>((int)Images.Image_Cursor).transform.position = Get<Image>(_currCursor).transform.position;
        }
        private void PushBagButton()
        {
            Debug.Log("PushBagButton");
        }
        private void PushTalismanButton()
        {
            Debug.Log("PushTalismanButton");
        }
        private void PushSettingButton()
        {
            UIManager.Instance.ShowNormalUI<UI_Setting>();
        }
        private void PushMainButton()
        {
            UIManager.Instance.Clear();
            _transition.TransitionInternal();
        }
        private void PushCloseButton()
        {
            Time.timeScale = 1;
            UIManager.Instance.CloseNormalUI(this);
        }
        #region Update
        private void Update()
        {
            HandleUIKeyInput();
        }
        private void HandleUIKeyInput()
        {
            if (!Input.anyKey)
                return;

            if (_uiNum != UIManager.Instance.UINum)
                return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PushCloseButton();
                return;
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                _actions[_currCursor].Invoke();
                return;
            }

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                _currCursor = (_currCursor + 1) % MENU_SIZE;
                Get<Image>((int)Images.Image_Cursor).transform.position = Get<Image>(_currCursor).transform.position;
                return;
            }

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _currCursor = (_currCursor - 1 + MENU_SIZE) % MENU_SIZE;
                Get<Image>((int)Images.Image_Cursor).transform.position = Get<Image>(_currCursor).transform.position;
                return;
            }
        }
        #endregion
    }
}
