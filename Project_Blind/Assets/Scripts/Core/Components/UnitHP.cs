using System;
using UnityEngine;

namespace Blind
{
    /// <summary>
    /// 유닛의 체력을 관리하는 클래스입니다. 
    /// </summary>
    public class UnitHP : IDamageable<float>
    {
        private float _health;
        private float _maxHealth;
        private bool isInvincibility = false;

        // SetHealth 실행시키면 여기에 등록된 함수들이 실행됨
        // UI_FieldScene에서 연동해놨음
        public Action<float , float> RefreshHpUI;

        public void SetHealth()
        {
            if(RefreshHpUI != null)
                RefreshHpUI.Invoke(_health, _maxHealth);
        }

        public void SetHealth(UnitHP hp)
        {
            _health = hp.GetHP();
            _maxHealth = hp.GetMaxHP();
            SetHealth();
        }

        public UnitHP(float maxHealth) {
            _maxHealth = maxHealth;
            _health = _maxHealth;

        }

        public UnitHP(float maxHealth, float currentHealth) :this(maxHealth)
        {
            _health = currentHealth;
            SetHealth();
        }
        public void GetDamage(float damage) {
            if(!isInvincibility)
                _health -= damage;
            if (_health < 0)
                _health = 0;

            SetHealth();
        }
        public void GetHeal(float heal) {
            if(_health + heal > _maxHealth) {
                _health = _maxHealth;
                return;
            }
            _health += heal;

            SetHealth();
        }
        public float GetHP() {
            return _health;
        }
        public float GetMaxHP()
        {
            return _maxHealth;
        }

        public void Invincibility()
        {
            isInvincibility = true;
            Debug.Log("무적!");
        }

        public void unInvicibility()
        {
            isInvincibility = false;
            Debug.Log("무적 풀림");
        }

        public void ResetHp()
        {
            _health = _maxHealth;
        }
    }
}