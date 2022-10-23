using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Blind
{
    public class UI_BossHp : UI_Base
    {
        float _hp;
        float _maxHp;
        Coroutine _coHpChange;
        BossEnemyCharacter boss;
        enum Images
        {
            Image_BossHp_AfterImage,
            Image_BossHp_RealHp
        }
        public override void Init()
        {
            Bind<Image>(typeof(Images));
            boss = FindObjectOfType<BossEnemyCharacter>();
            Debug.Log(boss.name);
            boss.Hp.RefreshHpUI -= OnHpChanged;
            boss.Hp.RefreshHpUI += OnHpChanged;
            
            _hp = boss.Hp.GetHP();
            _maxHp = boss.Hp.GetMaxHP();
            Get<Image>((int)Images.Image_BossHp_RealHp).fillAmount = _hp / _maxHp;
            Get<Image>((int)Images.Image_BossHp_AfterImage).fillAmount = _hp / _maxHp;
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
        IEnumerator CoChangeHp(float lastHp, float targetHp, float time)
        {
            float value = _maxHp;
            if (lastHp < targetHp)
            {
                while (true)
                {
                    if (_hp > targetHp)
                    {
                        _hp = targetHp;
                        Get<Image>((int)Images.Image_BossHp_RealHp).fillAmount = _hp / _maxHp;
                        Get<Image>((int)Images.Image_BossHp_AfterImage).fillAmount = _hp / _maxHp;
                        _coHpChange = null;
                        break;
                    }
                    _hp += value * Time.deltaTime;
                    Get<Image>((int)Images.Image_BossHp_RealHp).fillAmount = _hp / _maxHp;
                    Get<Image>((int)Images.Image_BossHp_AfterImage).fillAmount = _hp / _maxHp;
                    yield return new WaitForSeconds(time / 5f);
                }
            }
            else
            {
                Get<Image>((int)Images.Image_BossHp_RealHp).fillAmount = targetHp / _maxHp;
                while (true)
                {
                    if (_hp < targetHp)
                    {
                        _hp = targetHp;
                        Get<Image>((int)Images.Image_BossHp_AfterImage).fillAmount = _hp / _maxHp;
                        _coHpChange = null;
                        break;
                    }
                    _hp -= value * Time.deltaTime;
                    Get<Image>((int)Images.Image_BossHp_AfterImage).fillAmount = _hp / _maxHp;
                    yield return new WaitForSeconds(time / 5f);
                }
            }
        }
        private void OnDestroy()
        {
            boss.Hp.RefreshHpUI -= OnHpChanged;
        }
    }
}

