using System;
using UnityEngine;

namespace Blind
{
    public class AttackInfo
    {
        public float Damage { get; private set; }
        public Facing EnemyFacing;

        public AttackInfo(float damage,Facing enemyFacing)
        {
            Damage = damage;
            EnemyFacing = enemyFacing;
        }
    }
}