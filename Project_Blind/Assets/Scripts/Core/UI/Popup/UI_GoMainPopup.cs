using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Blind
{
    public class UI_GoMainPopup : UI_Popup
    {
        const int BUTTON_CNT = 2;
        TransitionPoint _transition = null;
        private Action[] _actions = new Action[BUTTON_CNT];
        enum Images
        {
            Button_Yes,
            Button_No
        }

        private int _currCursor;

        [SerializeField] private Sprite _sprite_Yes_Clicked;
        [SerializeField] private Sprite _sprite_Yes_NonClicked;
        [SerializeField] private Sprite _sprite_No_Clicked;
        [SerializeField] private Sprite _sprite_No_NonClicked;

        struct SpriteInfo
        {
            public int width;
            public int height;
            public Sprite sprite;
        }

        struct SpriteInfoSet
        {
            public SpriteInfo click;
            public SpriteInfo nonClick;
        }

        private SpriteInfo _spriteInfo_Yes_Clicked;
        private SpriteInfo _spriteInfo_Yes_NonClicked;
        private SpriteInfo _spriteInfo_No_Clicked;
        private SpriteInfo _spriteInfo_No_NonClicked;

        SpriteInfoSet[] _spriteInfoSets = new SpriteInfoSet[BUTTON_CNT];
        public override void Init()
        {
            base.Init();
            _transition = GameObject.Find("TransitionStart_Main").GetComponent<TransitionPoint>();

            Bind<Image>(typeof(Images));

            Get<Image>((int)Images.Button_Yes).gameObject.BindEvent(PushYesButton, Define.UIEvent.Click);
            Get<Image>((int)Images.Button_No).gameObject.BindEvent(PushNoButton, Define.UIEvent.Click);
            Get<Image>((int)Images.Button_Yes).gameObject.BindEvent(() => ChangeCursor((int)Images.Button_Yes), Define.UIEvent.Enter);
            Get<Image>((int)Images.Button_No).gameObject.BindEvent(() => ChangeCursor((int)Images.Button_No), Define.UIEvent.Enter);

            _spriteInfo_Yes_Clicked = new SpriteInfo() { width = 51, height = 24, sprite = _sprite_Yes_Clicked };
            _spriteInfo_Yes_NonClicked = new SpriteInfo() { width = 44, height = 21, sprite = _sprite_Yes_NonClicked };
            _spriteInfo_No_Clicked = new SpriteInfo() { width = 46, height = 23, sprite = _sprite_No_Clicked };
            _spriteInfo_No_NonClicked = new SpriteInfo() { width = 39, height = 20, sprite = _sprite_No_NonClicked };

            _spriteInfoSets[(int)Images.Button_Yes].click = _spriteInfo_Yes_Clicked;
            _spriteInfoSets[(int)Images.Button_Yes].nonClick = _spriteInfo_Yes_NonClicked;
            _spriteInfoSets[(int)Images.Button_No].click = _spriteInfo_No_Clicked;
            _spriteInfoSets[(int)Images.Button_No].nonClick = _spriteInfo_No_NonClicked;

            _actions[(int)Images.Button_Yes] += PushYesButton;
            _actions[(int)Images.Button_No] += PushNoButton;

            UIManager.Instance.KeyInputEvents -= HandleKeyInput;
            UIManager.Instance.KeyInputEvents += HandleKeyInput;

            EnterCursor((int)Images.Button_Yes);
        }
        private void HandleKeyInput()
        {
            if (!Input.anyKey)
                return;

            if (_uiNum != UIManager.Instance.UINum)
                return;

            if (Input.GetKeyDown(KeyCode.Return))
            {
                _actions[_currCursor].Invoke();
                return;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PushNoButton();
                return;
            }

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                ChangeCursor((_currCursor + 1) % BUTTON_CNT);
                return;
            }

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ChangeCursor((_currCursor - 1 + BUTTON_CNT) % BUTTON_CNT);
                return;
            }
        }
        private void PushYesButton()
        {
            SoundManager.Instance.Play("Select");
            Time.timeScale = 1;
            UIManager.Instance.Clear();
            _transition.TransitionInternal();
        }
        private void PushNoButton()
        {
            SoundManager.Instance.Play("Select");
            UIManager.Instance.KeyInputEvents -= HandleKeyInput;
            ClosePopupUI();
        }
        private void EnterCursor(int idx)
        {
            _currCursor = idx;
            Image currImage = Get<Image>(idx);
            SpriteInfo spriteInfo = _spriteInfoSets[idx].click;
            currImage.sprite = spriteInfo.sprite;
            currImage.rectTransform.sizeDelta = new Vector2(spriteInfo.width, spriteInfo.height);
        }
        private void ExitCursor(int idx)
        {
            Image currImage = Get<Image>(idx);
            SpriteInfo spriteInfo = _spriteInfoSets[idx].nonClick;
            currImage.sprite = spriteInfo.sprite;
            currImage.rectTransform.sizeDelta = new Vector2(spriteInfo.width, spriteInfo.height);
        }
        private void ChangeCursor(int nextCursor)
        {
            SoundManager.Instance.Play("CursorMove");
            ExitCursor(_currCursor);
            EnterCursor(nextCursor);
        }
    }
}

