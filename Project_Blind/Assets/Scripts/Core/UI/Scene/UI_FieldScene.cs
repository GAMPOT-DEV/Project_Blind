using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Blind
{
    public class UI_FieldScene : UI_Scene
    {
        private const int GAUGE_SIZE = 3;

        private int _currWaveGauge = 30;
        private float _hp;
        private float _maxHp;
        private PlayerCharacter _player = null;

        private int _currWaveIndex = 2;
        private Slider[] _waveGauges = new Slider[GAUGE_SIZE];

        private const float ALPHA = 50f;
        private float _chargeAlpha = ALPHA;

        Coroutine _coHpChange = null;
        Coroutine _coCharge = null;
        
        enum Texts
        {
            Text_Money
        }
        enum Images
        {
            Image_TestDamage,
            Image_TestHeal,

            Charge,
            Image_RealHp
        }
        enum Sliders
        {
            Slider_WaveGauge1,
            Slider_WaveGauge2,
            Slider_WaveGauge3,
            Slider_HP,
        }
        protected override void Start()
        {

        }
        public override void Init()
        {
            base.Init();
            Bind<Text>(typeof(Texts));
            Bind<Image>(typeof(Images));
            Bind<Slider>(typeof(Sliders));

            UIManager.Instance.KeyInputEvents -= HandleUIKeyInput;
            UIManager.Instance.KeyInputEvents += HandleUIKeyInput;

            _player = FindObjectOfType<PlayerCharacter>();
            //_hp = _player.Hp.GetHP();
            //_maxHp = _player.Hp.GetMaxHP();

            _waveGauges[0] = Get<Slider>((int)Sliders.Slider_WaveGauge1);
            _waveGauges[1] = Get<Slider>((int)Sliders.Slider_WaveGauge2);
            _waveGauges[2] = Get<Slider>((int)Sliders.Slider_WaveGauge3);
            //OnWaveGaugeChanged(_player.CurrentWaveGauge);

            //InitTexts();
            InitEvents();
            DisplayMoney();
        }
        private void InitTexts()
        {
            //Get<Text>((int)Texts.Text_HP).text = $"{_hp}/{_maxHp}";
        }
        private void InitEvents()
        {
            _player.Hp.RefreshHpUI += OnHpChanged;
            _player.OnWaveGaugeChanged += OnWaveGaugeChanged;
            // Test
            Get<Image>((int)Images.Image_TestDamage).gameObject.BindEvent(() => _player.Hit(5.0f), Define.UIEvent.Click);
            Get<Image>((int)Images.Image_TestHeal).gameObject.BindEvent(() => _player.Hp.GetHeal(1.0f), Define.UIEvent.Click);
            Get<Image>((int)Images.Image_TestHeal).gameObject.BindEvent(TestWaveGauge, Define.UIEvent.Click);

            Get<Image>((int)Images.Image_TestDamage).gameObject.BindEvent(() => DataManager.Instance.SubMoney(1), Define.UIEvent.Click);
            Get<Image>((int)Images.Image_TestHeal).gameObject.BindEvent(() => DataManager.Instance.AddMoney(1), Define.UIEvent.Click);
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
            if (_hp == hp) return;
            if (_coHpChange != null)
            {
                StopCoroutine(_coHpChange);
                _coHpChange = null;
            }
            float lastHp = _hp;
            _maxHp = maxHp;
            float time = 1f / (Mathf.Abs(hp - lastHp) * 10f);
            _coHpChange = StartCoroutine(CoChangeHp(lastHp, hp, time));
            //Get<Slider>((int)Sliders.Slider_HP).value = _hp / _maxHp;
        }
        private void OnWaveGaugeChanged(int gauge)
        {
            //if (gauge == _currWaveGauge) return;
            //int lastWaveGauge = _currWaveGauge;
            //int lastWaveIndex = _currWaveIndex;
            //int targetWaveGauge = gauge;
            //int targetWaveIndex = Mathf.Max(0, gauge - 1) / 10;
            //Debug.Log(targetWaveIndex);
            //int speed = 1 / Mathf.Abs(lastWaveGauge - targetWaveGauge);
            //if (lastWaveIndex == targetWaveIndex)
            //{
            //    // 같은 바 안에서 증가/감소
            //    Debug.Log("1");
            //    if(_currWaveGauge>targetWaveGauge)
            //        StartCoroutine(CoDecreaseWaveGauge(targetWaveGauge, targetWaveIndex, speed));
            //}
            //else if (lastWaveIndex < targetWaveIndex)
            //{
            //    // 왼쪽 바에서 오른쪽 바로 증가

            //}
            //else
            //{
            //    // 오른쪽 바에서 왼쪽 바로 감소
            //    Debug.Log("2");
            //    StartCoroutine(CoDecreaseWaveGauge(targetWaveGauge, targetWaveIndex, speed));
            //}
            _currWaveGauge = gauge;
            int idx = 0;
            while (gauge >= 10)
            {
                _waveGauges[idx++].value = 1.0f;
                gauge -= 10;
            }
            if (idx >= GAUGE_SIZE) return;
            _waveGauges[idx].value = (float)gauge / 10.0f;
            for (int i = idx + 1; i < GAUGE_SIZE; i++)
                _waveGauges[i].value = 0f;
        }
        IEnumerator CoIncreaseWaveGauge(int index, int time)
        {
            // TODO
            while (true)
            {

            }
        }
        IEnumerator CoDecreaseWaveGauge(int targetGauge, int targetIndex, int time)
        {
            // TODO
            while (true)
            {
                _currWaveGauge--;
                if (_currWaveGauge % 10 == 0) _currWaveIndex--;
                _waveGauges[_currWaveIndex].value = (float)(_currWaveGauge % 10) / 10;
                yield return new WaitForSeconds(0.5f);
                if (_currWaveIndex == targetIndex)
                {
                    if (_currWaveGauge == targetGauge)
                    {
                        Debug.Log(targetIndex);
                        Debug.Log(targetGauge);
                        _waveGauges[_currWaveIndex].value = (float)(_currWaveGauge % 11) / 10;
                        break;
                    }
                }
                else
                {
                    if (_currWaveGauge % 10 == 0)
                    {
                        _currWaveIndex--;
                        _waveGauges[_currWaveIndex].value = (float)(_currWaveGauge % 11) / 10;
                        StartCoroutine(CoDecreaseWaveGauge(targetGauge, targetIndex, time));
                        break;
                    }
                }
                
            }
        }
        private void TestWaveGauge()
        {
            _player.CurrentWaveGauge += 1;
        }
        public void StartCharge()
        {
            _coCharge = StartCoroutine(CoStartCharge());
        }
        IEnumerator CoChangeHp(float lastHp, float targetHp, float time)
        {
            float value = 0.1f;
            if (lastHp < targetHp)
            {
                while (true)
                {
                    if (_hp > targetHp)
                    {
                        _hp = targetHp;
                        Get<Image>((int)Images.Image_RealHp).fillAmount = _hp / _maxHp;
                        Get<Slider>((int)Sliders.Slider_HP).value = _hp / _maxHp;
                        _coHpChange = null;
                        break;
                    }
                    _hp += value * Time.deltaTime * 100;
                    Get<Image>((int)Images.Image_RealHp).fillAmount = _hp / _maxHp;
                    Get<Slider>((int)Sliders.Slider_HP).value = _hp / _maxHp;
                    yield return new WaitForSeconds(time / 5f);
                }
            }
            else
            {
                Get<Image>((int)Images.Image_RealHp).fillAmount = targetHp / _maxHp;
                while (true)
                {
                    if (_hp < targetHp)
                    {
                        _hp = targetHp;
                        Get<Slider>((int)Sliders.Slider_HP).value = _hp / _maxHp;
                        _coHpChange = null;
                        break;
                    }
                    _hp -= value * Time.deltaTime * 100;
                    Get<Slider>((int)Sliders.Slider_HP).value = _hp / _maxHp;
                    yield return new WaitForSeconds(time / 5f);
                }
            }
        }
        public void StopCharge()
        {
            if (_coCharge != null)
            {
                StopCoroutine(_coCharge);
                _coCharge = null;
            }
            _chargeAlpha = ALPHA;
            Get<Image>((int)Images.Charge).color = new Color(1f, 1f, 1f, _chargeAlpha / 255f);
        }
        IEnumerator CoStartCharge()
        {
            _chargeAlpha = ALPHA;
            while (true)
            {
                if (_chargeAlpha >= 255)
                {
                    _chargeAlpha = 255f;
                    Get<Image>((int)Images.Charge).color = new Color(1f, 1f, 1f, _chargeAlpha / 255f);
                    _coCharge = null;
                    break;
                }
                _chargeAlpha += 1.5f;
                Get<Image>((int)Images.Charge).color = new Color(1f, 1f, 1f, _chargeAlpha / 255f);
                yield return new WaitForSeconds(0.01f);
            }
        }
        public void DisplayMoney()
        {
            Get<Text>((int)Texts.Text_Money).text = DataManager.Instance.GameData.money.ToString();
        }
    }
}

