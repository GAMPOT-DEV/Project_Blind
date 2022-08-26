using UnityEngine;

namespace Blind.Boss
{
    public class BossPattern2: BossAttackPattern<FirstBossEnemy>
    {
        public override void AttackPattern()
        {
            Debug.Log("pattern2");
        }
    }
}