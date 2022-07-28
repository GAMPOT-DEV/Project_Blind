using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class BatMonster : MonoBehaviour
    {
        private enum State
        {
            Patrol,
            Default,
            Chase,
            Attack,
            AttackStandby,
            Hitted,
            Die
        }

        private CharacterController2D _characterController2D;
        private Rigidbody2D rigid;

        [SerializeField] private Vector2 _sensingRange;
        [SerializeField] private float _speed;
        [SerializeField] private float _runSpeed;
        [SerializeField] private float _attackCoolTime;
        [SerializeField] private float _attackSpeed;
        [SerializeField] private Vector2 _attackRange;
        [SerializeField] private int _MaxHP;

        private State state;
        private GameObject player;
        RaycastHit2D[] rayHit;
        public UnitHP HP;
        private MeleeAttackable _attack;
        public LayerMask WallLayer;
        public Transform WallCheck;
        private Transform startingPosition;
        private Vector2 patrolDirection;
        private PlayerFinder playerFinder;

        bool tmp = true;

        private void Awake()
        {
            _sensingRange = new Vector2(10f, 5f);
            _speed = 0.1f;
            _attackCoolTime = 0.5f;
            _attackRange = new Vector2(3f, 5f);
            _MaxHP = 10;

            _characterController2D = GetComponent<CharacterController2D>();
            rigid = GetComponent<Rigidbody2D>();
            state = State.Patrol;
            HP = new UnitHP(_MaxHP);
            patrolDirection = new Vector2(_speed, 0f);
            playerFinder = GetComponentInChildren<PlayerFinder>();
            playerFinder.setRange(_sensingRange);
        }
        private void Start()
        {
            startingPosition = gameObject.transform;
            player = GameObject.Find("Player");

            Debug.Log(patrolDirection);
        }

        
        private void FixedUpdate()
        {
            switch (state)
            {
                case State.Patrol:
                    updatePatrol();
                    break;

                case State.Default:
                    updateDefault();
                    break;

                case State.Chase:
                    updateChase();
                    break;

                case State.Attack:
                    updateAttack();
                    break;

                case State.AttackStandby:
                    updateAttackStandby();
                    break;

                case State.Hitted:
                    updateHitted();
                    break;

                case State.Die:
                    updateDie();
                    break;
            }
            _characterController2D.OnFixedUpdate();
        }

        private void updatePatrol()
        {
            _characterController2D.Move(patrolDirection);
            //FindTarget(); 
            if (tmp != playerFinder.FindPlayer())
            {
                Debug.Log(playerFinder.FindPlayer());
                tmp = playerFinder.FindPlayer();
            }
            if (Physics2D.OverlapCircle(WallCheck.position, 0.01f, WallLayer))
            {
                //state = State.Default;
                Flip();
            }
        }

        private void updateDefault()
        {

        }

        private void updateChase()
        {

        }

        private void updateAttack()
        {

        }

        private void updateAttackStandby()
        {

        }

        private void updateHitted()
        {

        }

        private void updateDie()
        {

        }

        public bool isAttack()
        {
            if (state == State.Attack)
                return true;
            else
                return false;
        }

        private void Flip()
        {
            Vector2 thisScale = transform.localScale;
            if (patrolDirection.x >= 0)
            {
                thisScale.x = -Mathf.Abs(thisScale.x);
                patrolDirection = new Vector2(-_speed, 0f);
            }
            else
            {
                thisScale.x = Mathf.Abs(thisScale.x);
                patrolDirection = new Vector2(_speed, 0f);
            }
            transform.localScale = thisScale;
        }

        private bool FindTarget()
        {
            
            return false;
        }
    }
}