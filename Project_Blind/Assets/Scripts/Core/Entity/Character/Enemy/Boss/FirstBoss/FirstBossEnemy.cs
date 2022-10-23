using System;
using System.Collections.Generic;
using System.Collections;
using Cinemachine;
using UnityEngine;
using Random = System.Random;

namespace Blind
{
    public class FirstBossEnemy : BossEnemyCharacter
    { 
        [SerializeField] private List<BossPhase> phaseList;
        [SerializeField] private List<Transform> AttackPosition;
        [SerializeField] private List<Transform> Pattern2AttackPosition;
        [SerializeField] private BoxCollider2D AttackRange;
        [SerializeField] private BoxCollider2D HitBox;
        [SerializeField] public Transform ShoutePatternPosition;
        [SerializeField] private Transform Pattern3Attackposition;
        public CinemachineImpulseSource _source;
        public Transform _floorStart;
        public Transform _floorEnd;
        private IEnumerator<BossPhase> _bossPhase;
        [SerializeField] private Transform point;
        private Animator _animator;
        private bool isStart = false;
        private int next;
        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
            SceneLinkedSMB<FirstBossEnemy>.Initialise(_animator, this);
            _animator.speed = 0f;
            gameObject.AddComponent<BossAttackPattern<FirstBossEnemy>>();
            _bossPhase = phaseList.GetEnumerator();
            _bossPhase.MoveNext();
            _source = GetComponent<CinemachineImpulseSource>();
            HitBox.gameObject.SetActive(false);
            foreach (var phase in phaseList)
            {
                
                phase.Init(this);
            }

            // Test
            ResourceManager.Instance.Instantiate("UI/Normal/UI_BossHp");
            Hp.SetHealth();
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
            _bossPhase.Current.Play();
        }

        public void NextPhase()
        {
            StartCoroutine(NextPhaseStart());
        }

        public void AttackInit(int x, int y, int damage)
        {
            AttackRange.gameObject.GetComponent<BossAttack>().ReAttackSize(new Vector2(x,y), damage);
        }

        IEnumerator NextPhaseStart()
        {
            yield return new WaitForSeconds(3f);
            StartCoroutine(NextPhaseScreenFade());
            yield return new WaitForSeconds(3f);
            BossPhaseStart();
        }

        public bool CheckForDead()
        {
            return Hp.GetHP() <= 0;
        }

        public void Dead()
        {
            _bossPhase.Current.Stop();
            _animator.SetBool("Dead", true);
        }
        public void Pattern2Start(int random)
        {
            AttackRange.gameObject.transform.position = Pattern2AttackPosition[random].position;
        }

        public void Pattern3Start()
        {
            AttackRange.gameObject.transform.position = Pattern3Attackposition.position;
        }
        
        public void Pattern4Start()
        {
            AttackRange.gameObject.transform.position = Pattern3Attackposition.position;
        }
        
        public void AttackNextPosition()
        {
            next = (next + 1) % AttackPosition.Count;
            AttackRange.gameObject.transform.position = AttackPosition[next].position;
        }

        public void enableAttack()
        {
            _source.GenerateImpulse();
            AttackRange.gameObject.GetComponent<BossAttack>().isAttack = true;
        }

        public void disableAttack()
        {
            AttackRange.gameObject.GetComponent<BossAttack>().isAttack = false;
        }

        public void ToggleRenderer(bool trigger)
        {
            _renderer.enabled = trigger;
        }
        IEnumerator NextPhaseScreenFade()
        {
            InputController.Instance.ReleaseControl(true);
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(UI_ScreenFader.FadeScenOut());
            yield return new WaitForSeconds(2.0f);
            
            _bossPhase.Current.End();
            _bossPhase.MoveNext();
            HitBox.gameObject.SetActive(true);
            
            yield return StartCoroutine(UI_ScreenFader.FadeSceneIn());
            InputController.Instance.GainControl();
            
        }

    }
}