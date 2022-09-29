using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Random = System.Random;

namespace Blind
{
    public class FirstBossEnemy : BossEnemyCharacter
    { 
        [SerializeField] private List<BossPhase> phaseList;
        public Transform _floorStart;
        public Transform _floorEnd;
        private IEnumerator<BossPhase> _bossPhase;
        protected override void Awake()
        {
            base.Awake();
            gameObject.AddComponent<BossAttackPattern<FirstBossEnemy>>();
            _bossPhase = phaseList.GetEnumerator();
            _bossPhase.MoveNext();
            foreach (var phase in phaseList)
            {
                phase.Init(this);
            }
        }
        public override void Reset()
        {
            base.Reset();
            if (_bossPhase.Current != null) _bossPhase.Current.Stop();
            _bossPhase.Reset();
        }

        public void Play()
        {
            if (_bossPhase.Current != null) _bossPhase.Current.Play();
        }
    }
}