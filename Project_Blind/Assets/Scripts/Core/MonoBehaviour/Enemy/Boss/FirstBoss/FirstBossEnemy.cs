using System;

namespace Blind
{
    public class FirstBossEnemy : BossEnemyCharacter
    {
        private IBossPhase Phase;
        
        private void Awake()
        {
            Init();
        }
    }
}