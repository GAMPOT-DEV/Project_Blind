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
        const int BUTTON_COUNT = (int)Images.Image_Exit + 1;
        private Action[] _actions = new Action[BUTTON_COUNT];
        private int _currCursor;
        TransitionPoint _transition;

        [SerializeField] private Sprite _startSprite_Click;
        [SerializeField] private Sprite _startSprite_NonClick;
        [SerializeField] private Sprite _settingSprite_Click;
        [SerializeField] private Sprite _settingSprite_NonClick;
        [SerializeField] private Sprite _ExitSprite_Click;
        [SerializeField] private Sprite _ExitSprite_NonClick;

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

        private ImageInfo _startImage_Click;
        private ImageInfo _startImage_NonClick;
        private ImageInfo _settingImage_Click;
        private ImageInfo _settingImage_NonClick;
        private ImageInfo _ExitImage_Click;
        private ImageInfo _ExitImage_NonClick;

        enum Images
        {
            Image_Start,
            Image_Option,
            Image_Exit,
            Image_Cursor,
            Image_Logo,
        }
        public override void Init()
        {
            base.Init();
            Debug.Log("Main Scene");
            Bind<Image>(typeof(Images));

            _startImage_Click = new ImageInfo() { width = 302, height = 41, sprite = _startSprite_Click };
            _startImage_NonClick = new ImageInfo() { width = 236, height = 34, sprite = _startSprite_NonClick };

            _settingImage_Click = new ImageInfo() { width = 259, height = 41, sprite = _settingSprite_Click };
            _settingImage_NonClick = new ImageInfo() { width = 202, height = 34, sprite = _settingSprite_NonClick };

            _ExitImage_Click = new ImageInfo() { width = 137, height = 48, sprite = _ExitSprite_Click };
            _ExitImage_NonClick = new ImageInfo() { width = 108, height = 41, sprite = _ExitSprite_NonClick };

            _imageInfos = new ImageInfoSet[BUTTON_COUNT];
            _imageInfos[(int)Images.Image_Start].click = _startImage_Click;
            _imageInfos[(int)Images.Image_Start].nonClick = _startImage_NonClick;
            _imageInfos[(int)Images.Image_Option].click = _settingImage_Click;
            _imageInfos[(int)Images.Image_Option].nonClick = _settingImage_NonClick;
            _imageInfos[(int)Images.Image_Exit].click = _ExitImage_Click;
            _imageInfos[(int)Images.Image_Exit].nonClick = _ExitImage_NonClick;

            Get<Image>((int)Images.Image_Start).gameObject.BindEvent(PushStartButton, Define.UIEvent.Click);
            Get<Image>((int)Images.Image_Option).gameObject.BindEvent(PushOptionButton, Define.UIEvent.Click);
            Get<Image>((int)Images.Image_Exit).gameObject.BindEvent(PushExitButton, Define.UIEvent.Click);

            Get<Image>((int)Images.Image_Start).gameObject.BindEvent(() => ChangeCursor((int)Images.Image_Start), Define.UIEvent.Enter);
            Get<Image>((int)Images.Image_Option).gameObject.BindEvent(() => ChangeCursor((int)Images.Image_Option), Define.UIEvent.Enter);
            Get<Image>((int)Images.Image_Exit).gameObject.BindEvent(() => ChangeCursor((int)Images.Image_Exit), Define.UIEvent.Enter);

            Get<Image>((int)Images.Image_Start).gameObject.BindEvent(() => ExitCursor((int)Images.Image_Start), Define.UIEvent.Exit);
            Get<Image>((int)Images.Image_Option).gameObject.BindEvent(() => ExitCursor((int)Images.Image_Option), Define.UIEvent.Exit);
            Get<Image>((int)Images.Image_Exit).gameObject.BindEvent(() => ExitCursor((int)Images.Image_Exit), Define.UIEvent.Exit);

            _currCursor = 0;
            Get<Image>(_currCursor).sprite = _startSprite_Click;
            Get<Image>((int)Images.Image_Cursor).transform.position = Get<Image>(_currCursor).transform.position;
            _actions[0] += PushStartButton;
            _actions[1] += PushOptionButton;
            _actions[2] += PushExitButton;

            Get<Image>((int)Images.Image_Logo).color = new Color(1, 1, 1, 0);
            StartCoroutine(CoApearLogoImage());

            _transition = FindObjectOfType<TransitionPoint>();
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
                return;

            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("Enter");
                _actions[_currCursor].Invoke();
                return;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                ChangeCursor((_currCursor + 1) % BUTTON_COUNT);
                return;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                ChangeCursor((_currCursor - 1 + BUTTON_COUNT) % BUTTON_COUNT);
                return;
            }
        }
        private void PushStartButton()
        {
            ChangeCursor((int)Images.Image_Start);
            UIManager.Instance.Clear();
            _transition.TransitionInternal();
        }
        private void PushOptionButton()
        {
            ChangeCursor((int)Images.Image_Option);
            UIManager.Instance.ShowNormalUI<UI_Setting>();
        }
        private void PushExitButton()
        {
            Debug.Log("Push Exit");
            ChangeCursor((int)Images.Image_Exit);
            Application.Quit();
        }
        private void EnterCursor(int idx)
        {
            _currCursor = idx;
            Get<Image>((int)Images.Image_Cursor).transform.position = Get<Image>(_currCursor).transform.position;
            Image currImage = Get<Image>(idx);
            ImageInfo imageInfo = _imageInfos[idx].click;
            currImage.sprite = imageInfo.sprite;
            currImage.rectTransform.sizeDelta = new Vector2(imageInfo.width, imageInfo.height);
        }
        private void ExitCursor(int idx)
        {
            Image currImage = Get<Image>(idx);
            ImageInfo imageInfo = _imageInfos[idx].nonClick;
            currImage.sprite = imageInfo.sprite;
            currImage.rectTransform.sizeDelta = new Vector2(imageInfo.width, imageInfo.height);
        }
        private void ChangeCursor(int nextCursor)
        {
            ExitCursor(_currCursor);
            EnterCursor(nextCursor);
        }
        IEnumerator CoApearLogoImage()
        {
            float alpha = 0;
            while (true)
            {
                alpha++;
                if (alpha >= 255)
                    break;
                Get<Image>((int)Images.Image_Logo).color = new Color(1, 1, 1, alpha / 255f);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}

