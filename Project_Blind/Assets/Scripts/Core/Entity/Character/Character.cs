using System;
using System.Collections;
using UnityEngine;

namespace Blind
{
    public abstract class Character : MonoBehaviour
    {
        [SerializeField] private float maxHp; // 초기화 시키기 위한 코드. 참조하지 말것 나중에 scriptableobject로 변환해야함
        private bool _isInvincibility;
        public UnitHP Hp { get; private set; }

        public void HittedWithKnockBack(AttackInfo attackInfo)
        {
            Hitted(attackInfo.Damage);
            HurtMove(attackInfo.EnemyFacing);
        }
        public void Hitted(float damage)
        {
            Hp.GetDamage(damage);
            if (Hp.GetHP() > 1)
            {
                onHurt();
            }
        }
        public void CharacterInvincible()
        {
            if(_isInvincibility) StartCoroutine(Invincibility());
        }

        protected abstract void onHurt();
        protected abstract void HurtMove(Facing enemyFacing);
        public bool CheckForDeed()
        {
            return Hp.GetHP()<= 0;
        }
        protected IEnumerator Invincibility()
        {
            _isInvincibility = true;
            Hp.Invincibility();
            yield return new WaitForSeconds(0.5f);
            Hp.unInvicibility();
            _isInvincibility = false;
            // 나중에 데미지관련 class만들어서 무적 넣을 예정
        }
        public virtual void Awake()
        {
            Hp = new UnitHP(maxHp);
        }
    }
}