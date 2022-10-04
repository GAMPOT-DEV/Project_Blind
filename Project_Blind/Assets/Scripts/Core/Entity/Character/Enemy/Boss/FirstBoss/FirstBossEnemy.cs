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
        [SerializeField] private Transform point;
        private Animator _animator;
        private bool isStart = false;
        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
            SceneLinkedSMB<FirstBossEnemy>.Initialise(_animator, this);
            _animator.speed = 0f;
            gameObject.AddComponent<BossAttackPattern<FirstBossEnemy>>();
            _bossPhase = phaseList.GetEnumerator();
            _bossPhase.MoveNext();
            foreach (var phase in phaseList)
            {
                phase.Init(this);
            }
        }

        protected override void FixedUpdate()
        {
            if (!isStart) return;
            
            transform.position = Vector2.MoveTowards(transform.position, point.position, 1f);

            if (transform.position.y == point.transform.position.y)
            {
                isStart = false;
                _animator.speed = 1f;
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
            if (_bossPhase.Current != null)
            {
                isStart = true;
                _animator.SetTrigger("Ganrim");
            }
        }

        public void BossPhaseStart()
        {
            Debug.Log("dd");
            _bossPhase.Current.Play();
        }
        
    }
}