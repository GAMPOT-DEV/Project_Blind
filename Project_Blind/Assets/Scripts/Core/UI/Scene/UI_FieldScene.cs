using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Blind
{
    public class UI_FieldScene : UI_Scene
    {
        
        private float _hp;
        private float _maxHp;
        private float _currWaveGauge;
        private float _maxWaveGauge;
        private PlayerCharacter _player = null;

        private const float ALPHA = 50f;
        private float _chargeAlpha = ALPHA;

        Coroutine _coHpChange = null;
        Coroutine _coWaveChange = null;
        Coroutine _coCharge = null;
        
        Dictionary<int, Data.BagItem> dict;
        enum Texts
        {
            Text_Money,
            Text_ItemCount1,
            Text_ItemCount2,
        }
        enum Images
        {
            Charge,
            Image_RealHp,
            Image_RealGauge,

            Image_ItemSlot1,
            Image_ItemSlot2
        }
        enum Sliders
        {
            Slider_WaveGauge,
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

            //OnWaveGaugeChanged(_player.CurrentWaveGauge);

            //InitTexts();
            InitEvents();
            DisplayMoney();

            dict = DataManager.Instance.BagItemDict;
            Get<Image>((int)Images.Image_ItemSlot1).sprite = ResourceManager.Instance.Load<Sprite>(dict[1].iconPath);
            Get<Image>((int)Images.Image_ItemSlot2).sprite = ResourceManager.Instance.Load<Sprite>(dict[2].iconPath);

            RefreshItemCnt();
        }
        private void InitTexts()
        {
            //Get<Text>((int)Texts.Text_HP).text = $"{_hp}/{_maxHp}";
        }
        private void InitEvents()
        {
            _player.Hp.RefreshHpUI += OnHpChanged;
            _player.OnWaveGaugeChanged += OnWaveGaugeChanged;
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

            if (Input.GetKeyDown(KeyCode.F1))
            {
                UIManager.Instance.ShowNormalUI<UI_Shop>();
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
            if (_currWaveGauge == gauge) return;
            if (_coWaveChange != null)
            {
                StopCoroutine(_coWaveChange);
                _coWaveChange = null;
            }
            float lastGauge = _currWaveGauge;
            _maxWaveGauge = 30f;
            float time = 1f / (Mathf.Abs((float)gauge - lastGauge) * 10f);
            _coWaveChange = StartCoroutine(CoChangeWave(lastGauge, gauge, time));

            //_currWaveGauge = gauge;
            //int idx = 0;
            //while (gauge >= 10)
            //{
            //    _waveGauges[idx++].value = 1.0f;
            //    gauge -= 10;
            //}
            //if (idx >= GAUGE_SIZE) return;
            //_waveGauges[idx].value = (float)gauge / 10.0f;
            //for (int i = idx + 1; i < GAUGE_SIZE; i++)
            //    _waveGauges[i].value = 0f;
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
            float value = 0.15f;
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
        IEnumerator CoChangeWave(float lastWave, float targetWave, float time)
        {
            float value = 0.15f;
            if (lastWave < targetWave)
            {
                while (true)
                {
                    if (_currWaveGauge > targetWave)
                    {
                        _currWaveGauge = targetWave;
                        Get<Image>((int)Images.Image_RealGauge).fillAmount = _currWaveGauge / _maxWaveGauge;
                        Get<Slider>((int)Sliders.Slider_WaveGauge).value = _currWaveGauge / _maxWaveGauge;
                        _coWaveChange = null;
                        break;
                    }
                    _currWaveGauge += value * Time.deltaTime * 150;
                    Get<Image>((int)Images.Image_RealGauge).fillAmount = _currWaveGauge / _maxWaveGauge;
                    Get<Slider>((int)Sliders.Slider_WaveGauge).value = _currWaveGauge / _maxWaveGauge;
                    yield return new WaitForSeconds(time / 5f);
                }
            }
            else
            {
                Get<Image>((int)Images.Image_RealGauge).fillAmount = targetWave / _maxWaveGauge;
                while (true)
                {
                    if (_currWaveGauge < targetWave)
                    {
                        _currWaveGauge = targetWave;
                        Get<Slider>((int)Sliders.Slider_WaveGauge).value = _currWaveGauge / _maxWaveGauge;
                        _coWaveChange = null;
                        break;
                    }
                    _currWaveGauge -= value * Time.deltaTime * 150;
                    Get<Slider>((int)Sliders.Slider_WaveGauge).value = _currWaveGauge / _maxWaveGauge;
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
        public void RefreshItemCnt()
        {
            BagItemInfo info1;
            DataManager.Instance.GameData.BagItemInfoById.TryGetValue(1, out info1);
            int itemCnt1 = 0;
            if (info1 != null) itemCnt1 = info1.itemCnt;
            Get<Text>((int)Texts.Text_ItemCount1).text = itemCnt1.ToString();

            BagItemInfo info2;
            DataManager.Instance.GameData.BagItemInfoById.TryGetValue(2, out info2);
            int itemCnt2 = 0;
            if (info2 != null) itemCnt2 = info2.itemCnt;
            Get<Text>((int)Texts.Text_ItemCount2).text = itemCnt2.ToString();
        }
    }
}

