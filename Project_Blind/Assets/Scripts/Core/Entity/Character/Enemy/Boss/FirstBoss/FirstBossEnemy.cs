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
        private IEnumerator _bossPhase;
        protected override void Awake()
        {
            base.Awake();
            gameObject.AddComponent<BossAttackPattern<FirstBossEnemy>>();
            _bossPhase = phaseList.GetEnumerator();
            foreach (var phase in phaseList)
            {
                phase.Init(this);
            }
        }

        private void Start()
        {
            ((BossPhase)_bossPhase.Current)?.Start();
        }
    }
}