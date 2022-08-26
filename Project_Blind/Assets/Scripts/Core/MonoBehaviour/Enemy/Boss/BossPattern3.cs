using UnityEngine;

namespace Blind.Boss
{
    public class BossPattern3: BossAttackPattern<Boss>
    {
        public override void AttackPattern()
        {
            Debug.Log("pattern3");
        }
    }
}