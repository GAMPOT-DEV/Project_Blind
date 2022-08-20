using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Blind
{
    public class UI_Menu : UI_Base
    {
        [SerializeField] private GameObject _settingUI;
        [SerializeField] private GameObject _clueUI;

        [SerializeField] private Sprite _bag_Clicked;
        [SerializeField] private Sprite _bag_NonClicked;
        [SerializeField] private Sprite _talisman_Clicked;
        [SerializeField] private Sprite _talisman_NonClicked;
        [SerializeField] private Sprite _clue_Clicked;
        [SerializeField] private Sprite _clue_NonClicked;
        [SerializeField] private Sprite _setting_Clicked;
        [SerializeField] private Sprite _setting_NonClicked;
        private GameObject _currActiveUI = null;
        #region Enums
        enum Images
        {
            Image_Bag,
            Image_Talisman,
            Image_Clue,
            Image_Setting,
            

            Image_Close,
        }
        #endregion
        const int MENU_SIZE = (int)Images.Image_Setting + 1;
        private Action[] _actions = new Action[MENU_SIZE];

        private int _currCursor;

        GameObject[] _uis = new GameObject[MENU_SIZE];

        struct SpriteInfo
        {
            public Sprite Clicked;
            public Sprite NonClicked;
        }

        SpriteInfo[] _sprites = new SpriteInfo[MENU_SIZE];

        TransitionPoint _transition;
        public override void Init()
        {
            Bind<Image>(typeof(Images));

            UIManager.Instance.KeyInputEvents -= HandleUIKeyInput;
            UIManager.Instance.KeyInputEvents += HandleUIKeyInput;

            InitEvents();
            Time.timeScale = 0;

            _transition = FindObjectOfType<TransitionPoint>();

            _settingUI.SetActive(false);
            _clueUI.SetActive(false);

            _uis[(int)Images.Image_Bag] = null;
            _uis[(int)Images.Image_Talisman] = null;
            _uis[(int)Images.Image_Clue] = _clueUI;
            _uis[(int)Images.Image_Setting] = _settingUI;

            _sprites[(int)Images.Image_Bag].Clicked = _bag_Clicked;
            _sprites[(int)Images.Image_Bag].NonClicked = _bag_NonClicked;
            _sprites[(int)Images.Image_Talisman].Clicked = _talisman_Clicked;
            _sprites[(int)Images.Image_Talisman].NonClicked = _talisman_NonClicked;
            _sprites[(int)Images.Image_Clue].Clicked = _clue_Clicked;
            _sprites[(int)Images.Image_Clue].NonClicked = _clue_NonClicked;
            _sprites[(int)Images.Image_Setting].Clicked = _setting_Clicked;
            _sprites[(int)Images.Image_Setting].NonClicked = _setting_NonClicked;

            PushButton(_currCursor);
        }
        private void InitEvents()
        {
            Get<Image>((int)Images.Image_Bag).gameObject.BindEvent(() => PushButton((int)Images.Image_Bag), Define.UIEvent.Click);
            Get<Image>((int)Images.Image_Talisman).gameObject.BindEvent(() => PushButton((int)Images.Image_Talisman), Define.UIEvent.Click);
            Get<Image>((int)Images.Image_Clue).gameObject.BindEvent(() => PushButton((int)Images.Image_Clue), Define.UIEvent.Click);
            Get<Image>((int)Images.Image_Setting).gameObject.BindEvent(() => PushButton((int)Images.Image_Setting), Define.UIEvent.Click);
            Get<Image>((int)Images.Image_Close).gameObject.BindEvent(PushCloseButton, Define.UIEvent.Click);

            _actions[(int)Images.Image_Bag] += PushBagButton;
            _actions[(int)Images.Image_Talisman] += PushTalismanButton;
            _actions[(int)Images.Image_Clue] += PushClueButton;
            _actions[(int)Images.Image_Setting] += PushSettingButton;
            

            _currCursor = (int)Images.Image_Bag;
            //Get<Image>((int)Images.Image_Cursor).transform.position = Get<Image>(_currCursor).transform.position;
        }
        private void PushButton(int num)
        {
            DataManager.Instance.SaveGameData();
            Get<Image>(_currCursor).sprite = _sprites[_currCursor].NonClicked;
            _currCursor = num;
            Get<Image>(_currCursor).sprite = _sprites[_currCursor].Clicked;
            //Get<Image>((int)Images.Image_Cursor).transform.position = Get<Image>(_currCursor).transform.position;
            ClearCurrActiveSetting();
            _actions[_currCursor].Invoke();
        }
        private void PushBagButton()
        {
            Debug.Log("PushBagButton");
        }
        private void PushTalismanButton()
        {
            Debug.Log("PushTalismanButton");
        }
        private void PushClueButton()
        {
            //UIManager.Instance.ShowNormalUI<UI_Clue>();
            _clueUI.SetActive(true);
            _currActiveUI = _clueUI;
        }
        private void PushSettingButton()
        {
            //UIManager.Instance.ShowNormalUI<UI_Setting>();
            _settingUI.SetActive(true);
            _currActiveUI = _settingUI;
        }
        private void PushCloseButton()
        {
            DataManager.Instance.SaveGameData();
            Time.timeScale = 1;
            _settingUI.GetComponent<UI_Setting>().CloseUI();
            UIManager.Instance.KeyInputEvents -= HandleUIKeyInput;
            UIManager.Instance.CloseNormalUI(this);
        }
        #region Update
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
                //Get<Image>((int)Images.Image_Cursor).transform.position = Get<Image>(_currCursor).transform.position;
                PushButton((_currCursor + 1) % MENU_SIZE);
                return;
            }

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                //Get<Image>((int)Images.Image_Cursor).transform.position = Get<Image>(_currCursor).transform.position;
                PushButton((_currCursor - 1 + MENU_SIZE) % MENU_SIZE);
                return;
            }
        }
        #endregion
        private void ClearCurrActiveSetting()
        {
            if (_currActiveUI != null)
            {
                if (_currActiveUI == _settingUI)
                {
                    _currActiveUI.GetComponent<UI_Setting>().CloseUI();
                }
                _currActiveUI.SetActive(false);
                _currActiveUI = null;
            }
        }
    }
}
