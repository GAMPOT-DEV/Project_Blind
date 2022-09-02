using System;
using UnityEngine;

namespace Blind
{
    public class BossPattern3: BossAttackPattern<FirstBossEnemy>

    {
        public override void AttackPattern()
        {
            Debug.Log("pattern3");
        }
    }
}