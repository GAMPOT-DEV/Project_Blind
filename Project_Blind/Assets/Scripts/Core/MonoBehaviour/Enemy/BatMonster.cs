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

        [SerializeField] private Vector2 _sensingRange = new Vector2(10f, 5f);
        [SerializeField] private float _speed = 0.1f;
        [SerializeField] private float _attackCoolTime = 0.5f;
        [SerializeField] private float _attackSpeed = 1f;
        [SerializeField] private Vector2 _attackRange = new Vector2(3f, 5f);
        [SerializeField] private int _MaxHP = 10;

        private State state;
        private GameObject player;
        RaycastHit2D[] rayHit;
        public UnitHP HP;
        private MeleeAttackable _attack;
        public LayerMask WallLayer;
        public Transform WallCheck;
        private Transform startingPosition;
        private Vector2 patrolDirection;

        private void Awake()
        {
            _characterController2D = GetComponent<CharacterController2D>();
            rigid = GetComponent<Rigidbody2D>();
            state = State.Patrol;
            HP = new UnitHP(_MaxHP);
            patrolDirection = new Vector2(_speed, 0f);
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
        }

        private void updatePatrol()
        {

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
    }
}