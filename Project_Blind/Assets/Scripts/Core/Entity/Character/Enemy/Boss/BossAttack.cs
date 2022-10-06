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

        public void OnTriggerStay2D(Collider2D col)
        {
            Debug.Log(isAttack);
            if(!isAttack) return;

            if (col.tag.Equals("Player"))
            {
                col.gameObject.GetComponent<Character>().HitWithKnockBack(new AttackInfo(damage,Facing.Right));
                isAttack = false;
            }
        }
    }
}