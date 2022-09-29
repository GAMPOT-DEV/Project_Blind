using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Blind
{
    [Serializable]
    public class FirstPhase : BossPhase
    {
        protected Random _rand = new Random();

        public void Init(FirstBossEnemy firstBossEnemy)
        {
            _parent = firstBossEnemy;
            _pattern = GetComponent<BossAttackPattern<FirstBossEnemy>>();
        }
        public void SetAttackPattern(BossAttackPattern<FirstBossEnemy> pattern)
        {
            _pattern = pattern;
            _pattern.Initialise(_parent);
        }
        public override void Start()
        {
            StartCoroutine(StartAttackState());
        }

        public override void End()
        {
        }
        public IEnumerator StartAttackState()
        {
            while (true)
            {
                var next = _rand.Next(0, 4);
                SetAttackPattern(_patternList[next]);
                yield return StartPattern();
                yield return new WaitForSeconds(1.5f);
            }
        }
        public Coroutine StartPattern()
        {
            return _pattern.AttackPattern();
        }
    }
}