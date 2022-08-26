using UnityEngine;

namespace Blind.Boss
{
    public class BossPattern4: BossAttackPattern<Boss>

    {
        public override void AttackPattern()
        {
            Debug.Log("pattern4");
        }
    }
}