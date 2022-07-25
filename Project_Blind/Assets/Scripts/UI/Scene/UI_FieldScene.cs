using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Blind
{
    public class UI_FieldScene : UI_Scene
    {
        float _hp;
        float _maxHp;
        PlayerCharacter _player = null;
        enum Texts
        {
            Text_HP,
        }
        enum Images
        {
            Image_TestDamage,
            Image_TestHeal,
        }
        public override void Init()
        {
            base.Init();
            Bind<Text>(typeof(Texts));
            Bind<Image>(typeof(Images));

            _player = FindObjectOfType<PlayerCharacter>();
            _hp = _player._damage.GetHP();
            _maxHp = _player._damage.GetMaxHP();

            InitTexts();
            InitEvents();
        }
        private void InitTexts()
        {
            Get<Text>((int)Texts.Text_HP).text = $"{_hp}/{_maxHp}";
        }
        private void InitEvents()
        {
            _player._damage.RefreshHpUI += OnHpChanged;

            // Test
            Get<Image>((int)Images.Image_TestDamage).gameObject.BindEvent(() => _player._damage.GetDamage(1.0f), Define.UIEvent.Click);
            Get<Image>((int)Images.Image_TestHeal).gameObject.BindEvent(() => _player._damage.GetHeal(1.0f), Define.UIEvent.Click);
        }
        private void Update()
        {
            HandleUIKeyInput();
        }
        private void HandleUIKeyInput()
        {
            if (!Input.anyKey)
                return;

            if (_uiNum != UIManager.Instance.UINum)
            {
                //Debug.Log(_uiNum);
                //Debug.Log(UIManager.Instance.UINum);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // TODO 메뉴 UI
                Debug.Log("ESC");
                UIManager.Instance.ShowNormalUI<UI_Menu>();
            }
        }
        private void OnHpChanged(float hp, float maxHp)
        {
            _hp = hp;
            _maxHp = maxHp;
            Get<Text>((int)Texts.Text_HP).text = $"{_hp}/{_maxHp}";
        }
    }
}

