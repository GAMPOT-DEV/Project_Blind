using System;
using UnityEngine;

namespace Blind
{
    public class BossPattern3: BossAttackPattern<FirstBossEnemy>

    {
        public override Coroutine AttackPattern()
        {
            Debug.Log("pattern3");
            return null;
        }
    }
}