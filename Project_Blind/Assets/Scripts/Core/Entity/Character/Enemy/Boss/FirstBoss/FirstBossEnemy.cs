using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Random = System.Random;

namespace Blind
{
    public class FirstBossEnemy : BossEnemyCharacter
    {
        private IBossPhase Phase;
        [SerializeField] private List<BossAttackPattern<FirstBossEnemy>> _patternList = new List<BossAttackPattern<FirstBossEnemy>>();
        public Transform _floorStart;
        public Transform _floorEnd;
        private BossAttackPattern<FirstBossEnemy> _pattern;
        private Random _rand = new Random();

        protected override void Awake()
        {
            base.Awake();
            gameObject.AddComponent<BossAttackPattern<FirstBossEnemy>>();
            _pattern = GetComponent<BossAttackPattern<FirstBossEnemy>>();
        }

        private void Start()
        {
            StartCoroutine(StartAttackState());
            ResourceManager.Instance.Instantiate("UI/Normal/UI_BossHp");
            Hp.SetHealth();
        }

        public IEnumerator StartAttackState()
        {
            while (true)
            {
                var next = _rand.Next(1, 5);
                ChangePattern(next);
                yield return StartPattern();
                yield return new WaitForSeconds(1.5f);

                // Test
                Hp.GetDamage(1);
            }
        }
        public void SetAttackPattern(BossAttackPattern<FirstBossEnemy> pattern)
        {
            _pattern = pattern;
            _pattern.Initialise(gameObject.GetComponent<FirstBossEnemy>());
        }

        public Coroutine StartPattern()
        {
            return _pattern.AttackPattern();
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