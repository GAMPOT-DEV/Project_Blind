using System;
using System.Collections;
using UnityEngine;

namespace Blind
{
    public abstract class Character : MonoBehaviour, IAttackFxExcutable
    {
        private bool _isInvincibility;
        public UnitHP Hp { get; protected set; }
        protected Renderer _renderer;
        public void Awake(ScriptableObjects.Character data)
        {
            Hp = new UnitHP(data.maxHp);
            _renderer = GetComponent<Renderer>();
        }

        public void HitWithKnockBack(AttackInfo attackInfo)
        {
            if (!_isInvincibility)
            {
                Hit(attackInfo.Damage);
                HurtMove(attackInfo.EnemyFacing);

            }
        }
        public void Hit(float damage)
        {
            var obj = ResourceManager.Instance.Instantiate("FX/HitFx/hit");
            obj.transform.position = transform.position + Vector3.up * 5;
            Hp.GetDamage(damage);
            if (Hp.GetHP() > 1 && !_isInvincibility)
            {
                onHurt();
            }
        }
        public void CharacterInvincible()
        {
            if(!_isInvincibility) StartCoroutine(Invincibility());
        }

        public abstract void HitSuccess();
        protected abstract void onHurt();
        protected abstract void HurtMove(Facing enemyFacing);
        public abstract Facing GetFacing();
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
        public void PlayAttackFx(int level, Facing face)
        {
            //Debug.Log(transform.GetChild(1).GetChild(level));
            transform.GetChild(1).GetChild(level).GetComponent<AttackFX>().Play(face);
        }
    }
}