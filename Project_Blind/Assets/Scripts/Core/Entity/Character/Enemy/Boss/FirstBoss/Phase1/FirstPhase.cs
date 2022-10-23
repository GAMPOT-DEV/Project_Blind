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
        private Coroutine _coroutine;
        private int _patternCount = 0;

        public override void Init(FirstBossEnemy firstBossEnemy)
        {
            base.Init(firstBossEnemy);
            _parent = firstBossEnemy;
            _pattern = GetComponent<BossAttackPattern<FirstBossEnemy>>();
            Debug.Log("페이즈 1");
        }
        public void SetAttackPattern(BossAttackPattern<FirstBossEnemy> pattern)
        {
            _pattern = pattern;
            _pattern.Initialise(_parent);
        }
        public override void Play()
        {
            transform.GetChild(3).gameObject.SetActive(true);
            _parent.ToggleRenderer(false);
            _patternCount = 0;
            SoundManager.Instance.Play("장산범/장산범 1페이즈", Define.Sound.Bgm);
            _coroutine = StartCoroutine(StartAttackState());
        }

        public override void End()
        {
            _parent.ToggleRenderer(true);
            transform.GetChild(3).gameObject.SetActive(false);
        }

        public override void Stop()
        {
            StopCoroutine(_coroutine);
        }

        public IEnumerator StartAttackState()
        {
            yield return new WaitForSeconds(3f);
            while (true)
            {
                if (_patternCount >= 6)
                {
                    Stop();
                    _parent.NextPhase();
                }
                var next = _rand.Next(0, 2);
                SetAttackPattern(_patternList[next]);
                yield return StartPattern();
                _patternCount++;
                yield return new WaitForSeconds(1.5f);
            }
        }
        public Coroutine StartPattern()
        {
            return _pattern.AttackPattern();
        }
    }
}