using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Blind
{
    public class UI_Setting : UI_Base
    {
        int tmp = 0;

        bool _vibration = true;

        int _screenMode = SIZE - 1;
        const int SIZE = 3;
        bool _isWindowMode;

        Define.Resolution[] _resolutions = new Define.Resolution[SIZE];
        #region Enums
        enum Texts
        {
            Text_Setting,
            Text_Audio,
            Text_MasterVolume,
            Text_BgmVolume,
            Text_EffectVolume,

            Text_Effect,
            Text_Vibration,
            Text_Vibration_OnOff,
            Text_MotionEffect,

            Text_KeySetting,
            Text_KeySettingButton,

            Text_Screen,
            Text_ScreenSize,
            Text_ScreenSizeValue,
            Text_ScreenMode,
            Text_ScreenMode_OnOff,
        }
        enum Images
        {
            Button_Close,

            Button_Vibration,

            Button_KeySetting,

            Button_ChangeScreenSize_Left,
            Button_ChangeScreenSize_Right,
            Button_ScreenMode,

            // Test
            TestBgm,
            TestEffect,
        }
        enum Sliders
        {
            Slider_MasterVolume,
            Slider_BgmVolume,
            Slider_EffectVolume,

            Slider_MotionEffect
        }
        #endregion
        public override void Init()
        {
            _uiNum = UIManager.Instance.UINum;
            Bind<Text>(typeof(Texts));
            Bind<Image>(typeof(Images));
            Bind<Slider>(typeof(Sliders));

            InitResolution();
            InitTexts();
            InitSliders();
            InitEvents();
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

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PushCloseButton();
                return;
            }
        }
        #region Initialize
        private void InitResolution()
        {
            _resolutions[0].width = 960;
            _resolutions[0].height = 540;

            _resolutions[1].width = 1280;
            _resolutions[1].height = 720;

            _resolutions[2].width = 1920;
            _resolutions[2].height = 1080;

            _isWindowMode = false;

            UIManager.Instance.Resolution = _resolutions[_screenMode];
            UIManager.Instance.IsWindowMode = _isWindowMode;
        }
        private void InitTexts()
        {
            // 사운드 관련
            Get<Text>((int)Texts.Text_Setting).text = "설정";
            Get<Text>((int)Texts.Text_Audio).text = "오디오";
            Get<Text>((int)Texts.Text_MasterVolume).text = "음량";
            Get<Text>((int)Texts.Text_BgmVolume).text = "음악";
            Get<Text>((int)Texts.Text_EffectVolume).text = "효과음";
            // 이펙트 관련 TODO
            Get<Text>((int)Texts.Text_Effect).text = "이펙트";
            Get<Text>((int)Texts.Text_Vibration).text = "화면 전체 진동";
            Get<Text>((int)Texts.Text_Vibration_OnOff).text = "On";
            Get<Text>((int)Texts.Text_MotionEffect).text = "모션 이펙트";
            // 키셋팅 관련 TODO
            Get<Text>((int)Texts.Text_KeySetting).text = "키셋팅";
            Get<Text>((int)Texts.Text_KeySettingButton).text = "키보드 설정";
            // 해상도 관련 TODO
            Get<Text>((int)Texts.Text_Screen).text = "해상도";
            Get<Text>((int)Texts.Text_ScreenSize).text = "해상도";
            Get<Text>((int)Texts.Text_ScreenSizeValue).text =
               $"{_resolutions[_screenMode].width} * {_resolutions[_screenMode].height}";
            Get<Text>((int)Texts.Text_ScreenMode).text = "창모드";
            Get<Text>((int)Texts.Text_ScreenMode_OnOff).text = _isWindowMode ? "On" : "Off";
        }
        private void InitSliders()
        {
            // 사운드 관련
            Get<Slider>((int)Sliders.Slider_MasterVolume).value = 1.0f;
            Get<Slider>((int)Sliders.Slider_BgmVolume).value = 1.0f;
            Get<Slider>((int)Sliders.Slider_EffectVolume).value = 1.0f;
            Get<Slider>((int)Sliders.Slider_MasterVolume).onValueChanged.AddListener(delegate { ChangeMasterVolume(); });
            Get<Slider>((int)Sliders.Slider_BgmVolume).onValueChanged.AddListener(delegate { ChangeVolume(Define.Sound.Bgm); });
            Get<Slider>((int)Sliders.Slider_EffectVolume).onValueChanged.AddListener(delegate { ChangeVolume(Define.Sound.Effect); });
            // 이펙트 관련 TODO
            // 키셋팅 관련 TODO
            // 해상도 관련 TODO
        }
        private void InitEvents()
        {
            // 사운드 관련
            Get<Image>((int)Images.Button_Close).gameObject.BindEvent(PushCloseButton, Define.UIEvent.Click);
            Get<Image>((int)Images.TestBgm).gameObject.BindEvent(PushTestBgmButton, Define.UIEvent.Click);
            Get<Image>((int)Images.TestEffect).gameObject.BindEvent(PushTestEffectButton, Define.UIEvent.Click);
            // 이펙트 관련 TODO
            Get<Image>((int)Images.Button_Vibration).gameObject.BindEvent(PushVibrationOnOffButton, Define.UIEvent.Click);
            // 키셋팅 관련 TODO
            Get<Image>((int)Images.Button_KeySetting).gameObject.BindEvent(PushKeySettingButton, Define.UIEvent.Click);
            // 해상도 관련 TODO
            Get<Image>((int)Images.Button_ChangeScreenSize_Right).gameObject.BindEvent(PushRightButton, Define.UIEvent.Click);
            Get<Image>((int)Images.Button_ChangeScreenSize_Left).gameObject.BindEvent(PushLeftButton, Define.UIEvent.Click);
            Get<Image>((int)Images.Button_ScreenMode).gameObject.BindEvent(PushWindowModeButton, Define.UIEvent.Click);
        }
        #endregion
        #region UI Event
        private void PushCloseButton()
        {
            SoundManager.Instance.StopBGM();
            UIManager.Instance.CloseNormalUI(this);
        }
        #endregion
        #region Sound Event
        private void PushTestBgmButton()
        {
            if (tmp == 0)
            {
                SoundManager.Instance.Play("Bgm1", Define.Sound.Bgm);
                tmp = 1;
            }
            else
            {
                SoundManager.Instance.StopBGM();
                tmp = 0;
            }
        }
        private void PushTestEffectButton()
        {
            SoundManager.Instance.Play("TestSound", Define.Sound.Effect);
        }
        private void ChangeMasterVolume()
        {
            SoundManager.Instance.ChangeMasterVolume(Get<Slider>((int)Sliders.Slider_MasterVolume).value);
        }
        private void ChangeVolume(Define.Sound sound)
        {
            switch (sound)
            {
                case Define.Sound.Bgm:
                    SoundManager.Instance.ChangeVolume(Define.Sound.Bgm, Get<Slider>((int)Sliders.Slider_BgmVolume).value);
                    break;
                case Define.Sound.Effect:
                    SoundManager.Instance.ChangeVolume(Define.Sound.Effect, Get<Slider>((int)Sliders.Slider_EffectVolume).value);
                    break;
            }
        }
        #endregion
        #region Effect Event
        private void PushVibrationOnOffButton()
        {
            if (_vibration)
            {
                Get<Text>((int)Texts.Text_Vibration_OnOff).text = "Off";
                _vibration = false;
            }
            else
            {
                Get<Text>((int)Texts.Text_Vibration_OnOff).text = "On";
                _vibration = true;
            }
        }
        #endregion
        #region KeySetting Event
        private void PushKeySettingButton()
        {
            Debug.Log("Push KeySetting Button");
        }
        #endregion
        #region ScreenEvent
        private void PushRightButton()
        {
            _screenMode = (_screenMode + 1) % SIZE;
            Get<Text>((int)Texts.Text_ScreenSizeValue).text =
                $"{_resolutions[_screenMode].width} * {_resolutions[_screenMode].height}";
            UIManager.Instance.Resolution = _resolutions[_screenMode];
        }
        private void PushLeftButton()
        {
            _screenMode = (_screenMode - 1 + SIZE) % SIZE;
            Get<Text>((int)Texts.Text_ScreenSizeValue).text =
               $"{_resolutions[_screenMode].width} * {_resolutions[_screenMode].height}";
            UIManager.Instance.Resolution = _resolutions[_screenMode];
        }
        private void PushWindowModeButton()
        {
            if (_isWindowMode)
            {
                _isWindowMode = false;
                Get<Text>((int)Texts.Text_ScreenMode_OnOff).text = _isWindowMode ? "On" : "Off";
                UIManager.Instance.IsWindowMode = _isWindowMode;
            }
            else
            {
                _isWindowMode = true;
                Get<Text>((int)Texts.Text_ScreenMode_OnOff).text = _isWindowMode ? "On" : "Off";
                UIManager.Instance.IsWindowMode = _isWindowMode;
            }
        }
        #endregion
    }
}