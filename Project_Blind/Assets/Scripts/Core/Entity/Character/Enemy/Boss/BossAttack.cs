using System;
using UnityEngine;

namespace Blind
{
    public class BossAttack: MonoBehaviour
    {
        private int x;
        private int y;
        private int damage;
        private BoxCollider2D _col;
        public bool isAttack = false;
        public void Init(int x, int y, int damage)
        {
            _col = GetComponent<BoxCollider2D>();
            _col.size = new Vector2(x,y);
            this.damage = damage;
        }

        public void ReAttackSize(Vector2 size, int damage)
        {
            gameObject.GetComponent<BoxCollider2D>().size = size;
            this.damage = damage;
        }

        public void OnTriggerStay2D(Collider2D col)
        {
            if(!isAttack) return;

            if (col.tag.Equals("Player"))
            {
                SoundManager.Instance.Play("타격-무겁게", Define.Sound.Effect);
                col.gameObject.GetComponent<Character>().HitWithKnockBack(new AttackInfo(damage,Facing.Right));
                isAttack = false;
            }
        }
    }
}