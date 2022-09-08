using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace Blind
{
    public class FirstBossEnemy : BossEnemyCharacter
    {
        private IBossPhase Phase;
        [SerializeField] private List<BossAttackPattern<FirstBossEnemy>> _patternList = new List<BossAttackPattern<FirstBossEnemy>>();
        public Transform _floorStart;
        public Transform _floorEnd;
        private BossAttackPattern<FirstBossEnemy> _pattern;

        private void Awake()
        {
            base.Awake();
            gameObject.AddComponent<BossAttackPattern<FirstBossEnemy>>();
            _pattern = GetComponent<BossAttackPattern<FirstBossEnemy>>();
            ChangePattern(1);
            StartPattern();
        }
        public void SetAttackPattern(BossAttackPattern<FirstBossEnemy> pattern)
        {
            _pattern = pattern;
            _pattern.Initialise(gameObject.GetComponent<FirstBossEnemy>());
        }

        public void StartPattern()
        {
            _pattern.AttackPattern();
        }

        IEnumerator BossPatternTest()
        {
            ChangePattern(1);
            yield return new WaitForSeconds(1.5f);
            ChangePattern(2);
            yield return new WaitForSeconds(1.5f);
            ChangePattern(3);
            yield return new WaitForSeconds(1.5f);
            ChangePattern(4);
        }
        public void ChangePattern(int pattern)
        {
            switch (pattern)
            {
                case 1:
                    SetAttackPattern(_patternList[0]);
                    break;
                case 2:
                    SetAttackPattern(_patternList[1]);
                    break;
                case 3:
                    SetAttackPattern(_patternList[2]);
                    break;
                case 4:
                    SetAttackPattern(_patternList[3]);
                    break;
            }
        }
    }
}