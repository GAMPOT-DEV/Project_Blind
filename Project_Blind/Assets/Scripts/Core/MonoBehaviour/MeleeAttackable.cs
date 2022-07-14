using System;
using UnityEngine;

namespace Blind
{
    public class MeleeAttackable: MonoBehaviour
    {
        private int x = 0;
        private int y = 0;
        private bool canDamage;
        public void init(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        private void FixedUpdate()
        {
            if (!canDamage) return;
            
            
        }
    }
}