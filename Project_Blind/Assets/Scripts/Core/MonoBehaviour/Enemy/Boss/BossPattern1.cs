using UnityEngine;

namespace Blind.Boss
{
    public class BossPattern1 : BossAttackPattern<Boss>
    {
        public override void AttackPattern()
        {
            Debug.Log("pattern1");
        }
    }
}