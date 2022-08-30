using UnityEngine;

namespace Blind.Boss
{
    public class BossPattern3: BossAttackPattern<FirstBossEnemy>
    {
        public override void AttackPattern()
        {
            Debug.Log("pattern3");
        }
    }
}