using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Blind
{
    public class UI_SavePoint : UI_Base
    {
        const int BUTTON_CNT = (int)Buttons.Button_GoBack + 1;
        private Action[] _actions = new Action[BUTTON_CNT];
        private int _currCursor;
        private PlayerCharacter player;
        enum Buttons
        {
            Button_Save,
            Button_Shop,
            Button_GoBack,
        }
        [SerializeField] private Sprite Image_Save_Non_Clicked;
        [SerializeField] private Sprite Image_Save_Clicked;
        [SerializeField] private Sprite Image_Shop_Non_Clicked;
        [SerializeField] private Sprite Image_Shop_Clicked;
        [SerializeField] private Sprite Image_GoBack_Non_Clicked;
        [SerializeField] private Sprite Image_GoBack_Clicked;

        struct ImageInfo
        {
            public int width;
            public int height;
            public Sprite sprite;
        }
        struct ImageInfoSet
        {
            public ImageInfo click;
            public ImageInfo nonClick;
        }
        ImageInfoSet[] _imageInfos;

        private ImageInfo ImageInfo_Save_Non_Clicked;
        private ImageInfo ImageInfo_Save_Clicked;
        private ImageInfo ImageInfo_Shop_Non_Clicked;
        private ImageInfo ImageInfo_Shop_Clicked;
        private ImageInfo ImageInfo_GoBack_Non_Clicked;
        private ImageInfo ImageInfo_GoBack_Clicked;

        public override void Init()
        {
            Time.timeScale = 0;
            player = FindObjectOfType<PlayerCharacter>();
            Bind<Button>(typeof(Buttons));
            SoundManager.Instance.Play("tower_in");

            UIManager.Instance.KeyInputEvents -= HandleKeyInput;
            UIManager.Instance.KeyInputEvents += HandleKeyInput;

            ImageInfo_Save_Non_Clicked = new ImageInfo() { width = 94, height = 48, sprite = Image_Save_Non_Clicked };
            ImageInfo_Save_Clicked = new ImageInfo() { width = 615, height = 70, sprite = Image_Save_Clicked };
            ImageInfo_Shop_Non_Clicked = new ImageInfo() { width = 196, height = 47, sprite = Image_Shop_Non_Clicked };
            ImageInfo_Shop_Clicked = new ImageInfo() { width = 614, height = 69, sprite = Image_Shop_Clicked };
            ImageInfo_GoBack_Non_Clicked = new ImageInfo() { width = 183, height = 48, sprite = Image_GoBack_Non_Clicked };
            ImageInfo_GoBack_Clicked = new ImageInfo() { width = 614, height = 63, sprite = Image_GoBack_Clicked };

            _imageInfos = new ImageInfoSet[BUTTON_CNT];
            _imageInfos[(int)Buttons.Button_Save].nonClick = ImageInfo_Save_Non_Clicked;
            _imageInfos[(int)Buttons.Button_Save].click = ImageInfo_Save_Clicked;
            _imageInfos[(int)Buttons.Button_Shop].nonClick = ImageInfo_Shop_Non_Clicked;
            _imageInfos[(int)Buttons.Button_Shop].click = ImageInfo_Shop_Clicked;
            _imageInfos[(int)Buttons.Button_GoBack].nonClick = ImageInfo_GoBack_Non_Clicked;
            _imageInfos[(int)Buttons.Button_GoBack].click = ImageInfo_GoBack_Clicked;

            Get<Button>((int)Buttons.Button_Save).gameObject.BindEvent(PushSaveButton, Define.UIEvent.Click);
            Get<Button>((int)Buttons.Button_Shop).gameObject.BindEvent(PushShopButton, Define.UIEvent.Click);
            Get<Button>((int)Buttons.Button_GoBack).gameObject.BindEvent(PushGoBackButton, Define.UIEvent.Click);
                     
            Get<Button>((int)Buttons.Button_Save).gameObject.BindEvent(() => ChangeCursor((int)Buttons.Button_Save), Define.UIEvent.Enter);
            Get<Button>((int)Buttons.Button_Shop).gameObject.BindEvent(() => ChangeCursor((int)Buttons.Button_Shop), Define.UIEvent.Enter);
            Get<Button>((int)Buttons.Button_GoBack).gameObject.BindEvent(() => ChangeCursor((int)Buttons.Button_GoBack), Define.UIEvent.Enter);
                     
            Get<Button>((int)Buttons.Button_Save).gameObject.BindEvent(() => ExitCursor((int)Buttons.Button_Save), Define.UIEvent.Exit);
            Get<Button>((int)Buttons.Button_Shop).gameObject.BindEvent(() => ExitCursor((int)Buttons.Button_Shop), Define.UIEvent.Exit);
            Get<Button>((int)Buttons.Button_GoBack).gameObject.BindEvent(() => ExitCursor((int)Buttons.Button_GoBack), Define.UIEvent.Exit);

            _currCursor = 0;
            ImageInfo info = _imageInfos[_currCursor].click;
            Get<Button>(_currCursor).image.sprite = info.sprite;
            Get<Button>(_currCursor).image.rectTransform.sizeDelta = new Vector2(info.width, info.height);
            _actions[0] += PushSaveButton;
            _actions[1] += PushShopButton;
            _actions[2] += PushGoBackButton;
        }
        private void HandleKeyInput()
        {
            if (!Input.anyKey)
                return;

            if (_uiNum != UIManager.Instance.UINum)
                return;

            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("Enter");
                _actions[_currCursor].Invoke();
                return;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _currCursor = (int)Buttons.Button_GoBack;
                PushGoBackButton();
                return;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                ChangeCursor((_currCursor + 1) % BUTTON_CNT);
                return;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                ChangeCursor((_currCursor - 1 + BUTTON_CNT) % BUTTON_CNT);
                return;
            }
        }
        private void PushSaveButton()
        {
            Debug.Log("Save");
            // 체력회복, 저장
            SavePoint save = FindObjectOfType<SavePoint>();
            if (save != null) save.DoInteraction();
            UI_ScreenConversation ui = UIManager.Instance.ShowNormalUI<UI_ScreenConversation>();
            ui.SetName("석탑");
            ui.SetScriptTitle(Define.ScriptTitle.RestScript);
            ui.StopMove = false;
            ui.AutoDisappear = true;
            PushGoBackButton();
        }
        private void PushShopButton()
        {
            Debug.Log("Shop");
            SoundManager.Instance.Play("Select");
        }
        private void PushGoBackButton()
        {
            Debug.Log("GoBack");
            player.UnTalk();
            Time.timeScale = 1;
            if (_currCursor == (int)Buttons.Button_Save) SoundManager.Instance.Play("Recover");
            else SoundManager.Instance.Play("Select");

            UIManager.Instance.CloseNormalUI(this);
            UIManager.Instance.KeyInputEvents -= HandleKeyInput;
        }
        private void EnterCursor(int idx)
        {
            _currCursor = idx;
            Button currImage = Get<Button>(idx);
            ImageInfo imageInfo = _imageInfos[idx].click;
            currImage.image.sprite = imageInfo.sprite;
            currImage.image.rectTransform.sizeDelta = new Vector2(imageInfo.width, imageInfo.height);
        }
        private void ExitCursor(int idx)
        {
            Button currImage = Get<Button>(idx);
            ImageInfo imageInfo = _imageInfos[idx].nonClick;
            currImage.image.sprite = imageInfo.sprite;
            currImage.image.rectTransform.sizeDelta = new Vector2(imageInfo.width, imageInfo.height);
        }
        private void ChangeCursor(int nextCursor)
        {
            SoundManager.Instance.Play("CursorMove");
            ExitCursor(_currCursor);
            EnterCursor(nextCursor);
        }
    }
}

