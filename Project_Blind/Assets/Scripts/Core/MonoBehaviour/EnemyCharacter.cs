using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class EnemyCharacter : MonoBehaviour
    {
        private enum State
        {
            Patrol,
            Default,
            Chase,
            Attack,
            Hitted,
            Die,
            Test
        }
        private CharacterController2D _characterController2D;
        private Rigidbody2D rigid;
        private float _recognitionRange = 5f;
        private Vector2 _patorlDirection;
        private Vector2 _startingPosition;
        private GameObject _player;
        private State state;
        private float _speed = 0.1f;
        RaycastHit2D[] rayHit;


        public UnitHP _damage;
        private MeleeAttackable _attack;
        public bool isAttack = false;


        private float _currentHP = 10f;

        public LayerMask layerMask;
        public Transform WallCheck;
        //Start 위치를 기준으로 투명벽을 그때그때 생성하는 것으로 변경 예정

        float timer;
        int waitTime = 1;

        private void Start()
        {
            _characterController2D = GetComponent<CharacterController2D>();
            rigid = GetComponent<Rigidbody2D>();
            _startingPosition = transform.position;
            _player = GameObject.Find("Player");
            state = State.Patrol;
            //state = State.Test;
            _damage = new UnitHP(10);
            _patorlDirection = new Vector2(_speed, 0f);
            //_attack = GetComponent<MeleeAttackable>();
            //_attack.Init(10, 10);
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            rayHit = Physics2D.RaycastAll(gameObject.transform.position, _patorlDirection, 50);
            Debug.DrawRay(gameObject.transform.position, _patorlDirection * 50, new Color(1, 0, 0));
            

            switch (state)
            {
                case State.Patrol:
                    _characterController2D.Move(_patorlDirection);
                    FindTarget();
                    if (Physics2D.OverlapCircle(WallCheck.position, 0.01f, layerMask))
                    {
                        timer = 0f;
                        state = State.Default;
                        //Flip();
                    }

                    break;

                case State.Default:
                    //잠시 가만히 서있는 모션으로 대기 (애니메이션 두 번 정도)
                    _characterController2D.Move(Vector2.zero);

                    timer += Time.deltaTime;
                    if (timer > waitTime)
                    {
                        Flip();
                        state = State.Patrol;
                    }

                    break;

                case State.Chase:
                    _characterController2D.Move(new Vector2(DirectionVector(_player.transform.position), 0f));
                    //MissTarget();
                    if (MissTarget())
                    {
                        state = State.Patrol;
                    }
                    else if (Physics2D.OverlapCircle(WallCheck.position, 0.01f, layerMask))
                    {
                        state = State.Default;
                    }
                    else if (Physics2D.OverlapCircle(WallCheck.position, 0.01f, 3))
                    {
                        state = State.Attack;
                    }
                    break;

                case State.Attack:
                    Debug.Log("Enemy Attack !");
                    isAttack = true;
                    StartCoroutine(AttackEnd());
                    break;

                case State.Hitted:
                    Vector2 hittedVelocity = Vector2.zero;
                    if (DirectionVector(_player.transform.position) > 0) //플레이어가 오른쪽
                    {
                        hittedVelocity = new Vector2(-200, 400);
                    }
                    else
                    {
                        hittedVelocity = new Vector2(200, 400);
                    }

                    rigid.AddForce(hittedVelocity);


                    timer = 0;
                    state = State.Chase;

                    break;

                case State.Die:
                    break;

                    /* Return 필요없을 것 같아서 일단 보류
                case State.Return:
                    float direction = DirectionVector(_startingPosition);
                    if (direction == 0)
                    {
                        state = State.Patrol;
                        break;
                    }
                    _characterController2D.Move(new Vector2(direction, 0f));
                    FindTarget();
                    break;
                    */
            }

            _characterController2D.OnFixedUpdate();
        }

        private float DirectionVector(Vector2 goal) //이동 방향만 지정
        {
            if (goal.x - transform.position.x > 0)
                return _speed * 2;
            else if (goal.x - transform.position.x < 0)
                return -_speed * 2;
            else if (Mathf.Abs(goal.x - transform.position.x) < 0.5f)
            {
                Debug.Log(Mathf.Abs(goal.x - transform.position.x));
                return 0;
            }
            return 0;
        }

        private bool FindTarget() //Player를 발견했을 때
        {
            /*
            if (Vector2.Distance(transform.position, _player.transform.position) < _recognitionRange)
            {
                state = State.Chase;
                return true;
            }
            */
            for(int i=0; i < rayHit.Length; i++)
            {
                if (rayHit[i].collider.tag == "Player")
                {
                    state = State.Chase;
                    Debug.Log(i + " Player " + state);
                    return true;
                }
            }
            return false;
        }

        private bool MissTarget() //Player가 추적 범위 밖으로 나갔을 때 -> 원위치로 되돌아옴
        {
            if (Vector2.Distance(transform.position, _player.transform.position) > _recognitionRange)
            {
                return true;
            }

            return false;
        }

        private void Flip()
        {
            Vector2 thisScale = transform.localScale;
            if (_patorlDirection.x >= 0)
            {
                thisScale.x = -Mathf.Abs(thisScale.x);
                _patorlDirection = new Vector2(-_speed, 0f);
            }
            else
            {
                thisScale.x = Mathf.Abs(thisScale.x);
                _patorlDirection = new Vector2(_speed, 0f);
            }
            transform.localScale = thisScale;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
                state = State.Hitted;
        }

        private IEnumerator AttackEnd()
        {
            yield return new WaitForSeconds(0.7f);
            isAttack = false;
            state = State.Chase;
        }
    }
}