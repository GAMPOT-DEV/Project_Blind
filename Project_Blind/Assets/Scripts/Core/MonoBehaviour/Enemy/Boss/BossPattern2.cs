using UnityEngine;

namespace Blind.Boss
{
    public class BossPattern2: BossAttackPattern<Boss>
    {
        public override void AttackPattern()
        {
            Debug.Log("pattern2");
        }
    }
}