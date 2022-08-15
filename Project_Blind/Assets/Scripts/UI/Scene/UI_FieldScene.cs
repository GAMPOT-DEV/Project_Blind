using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Blind
{
    public class UI_FieldScene : UI_Scene
    {
        private const int GAUGE_SIZE = 3;

        private float _hp;
        private float _maxHp;
        private PlayerCharacter _player = null;
        private Image[] _waveGauges = new Image[GAUGE_SIZE];
        enum Texts
        {
            Text_HP,
        }
        enum Images
        {
            Image_TestDamage,
            Image_TestHeal,

            Image_WaveGauge1,
            Image_WaveGauge2,
            Image_WaveGauge3,
        }
        protected override void Start()
        {
            EnemyCharacter monster = FindObjectOfType<EnemyCharacter>();
            if (monster != null)
            {
                Get<Image>((int)Images.Image_TestDamage).gameObject.BindEvent(() => monster.HP.GetDamage(1.0f), Define.UIEvent.Click);
                Get<Image>((int)Images.Image_TestHeal).gameObject.BindEvent(() => monster.HP.GetHeal(1.0f), Define.UIEvent.Click);
            }
        }
        public override void Init()
        {
            base.Init();
            Bind<Text>(typeof(Texts));
            Bind<Image>(typeof(Images));

            UIManager.Instance.KeyInputEvents -= HandleUIKeyInput;
            UIManager.Instance.KeyInputEvents += HandleUIKeyInput;

            _player = FindObjectOfType<PlayerCharacter>();
            _hp = _player.HpCenter.GetHP();
            _maxHp = _player.HpCenter.GetMaxHP();

            _waveGauges[0] = Get<Image>((int)Images.Image_WaveGauge1);
            _waveGauges[1] = Get<Image>((int)Images.Image_WaveGauge2);
            _waveGauges[2] = Get<Image>((int)Images.Image_WaveGauge3);
            OnWaveGaugeChanged(_player.CurrentWaveGauge);

            InitTexts();
            InitEvents();
        }
        private void InitTexts()
        {
            Get<Text>((int)Texts.Text_HP).text = $"{_hp}/{_maxHp}";
        }
        private void InitEvents()
        {
            _player.HpCenter.RefreshHpUI += OnHpChanged;
            _player.OnWaveGaugeChanged += OnWaveGaugeChanged;
            // Test
            Get<Image>((int)Images.Image_TestDamage).gameObject.BindEvent(() => _player.HpCenter.GetDamage(1.0f), Define.UIEvent.Click);
            Get<Image>((int)Images.Image_TestDamage).gameObject.BindEvent(() => _player.OnHurt(), Define.UIEvent.Click);
            Get<Image>((int)Images.Image_TestHeal).gameObject.BindEvent(() => _player.HpCenter.GetHeal(1.0f), Define.UIEvent.Click);
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
        private void OnWaveGaugeChanged(float gauge)
        {
            int cnt = (int)gauge;
            // 게이지 만큼 채운다
            for(int i = 0; i < cnt; i++)
            {
                _waveGauges[i].color = new Color(0, 0, 1, 1);
            }
            for(int i = cnt; i < GAUGE_SIZE; i++)
            {
                _waveGauges[i].color = new Color(1, 1, 1, 1);
            }
        }
    }
}

