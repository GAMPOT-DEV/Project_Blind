using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public abstract class BossPhase :MonoBehaviour 
    {
        [SerializeField] protected List<BossAttackPattern<FirstBossEnemy>> _patternList;
        protected BossAttackPattern<FirstBossEnemy> _pattern;
        protected FirstBossEnemy _parent;
        enum BossPhaseState
        {
            Pattern
        };

        public void Init(FirstBossEnemy firstBossEnemy)
        {
            _parent = firstBossEnemy;
        }
        public abstract void Start();
        public abstract void End();
    }
}