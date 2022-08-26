using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind.Boss
{
    public class Boss: MonoBehaviour
    {
        private BossAttackPattern<Boss> _pattern;
        [SerializeField] private List<BossAttackPattern<Boss>> _patternList = new List<BossAttackPattern<Boss>>();
        public void Awake()
        {
            gameObject.AddComponent<BossAttackPattern<Boss>>();
            _pattern = GetComponent<BossAttackPattern<Boss>>();
        }

        public void Start()
        {
            StartCoroutine(BossPatternTest());
        }

        public void FixedUpdate()
        {
            StartPattern();
        }

        public void SetAttackPattern(BossAttackPattern<Boss> pattern)
        {
            _pattern = pattern;
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