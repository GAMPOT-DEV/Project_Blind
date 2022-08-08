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
            AttackStandby,
            Hitted,
            Die
        }

        protected CharacterController2D _characterController2D;
        protected Rigidbody2D rigid;
        protected SpriteRenderer _sprite;

        [SerializeField] protected Vector2 _sensingRange;
        [SerializeField] protected float _speed;
        [SerializeField] protected float _runSpeed;
        [SerializeField] protected float _attackCoolTime;
        [SerializeField] protected float _attackSpeed;
        [SerializeField] protected Vector2 _attackRange;
        [SerializeField] protected int _maxHP;
        [SerializeField] protected int _damage;
        [SerializeField] protected float _stunTime;

        protected GameObject player;
        RaycastHit2D[] rayHit;
        public UnitHP HP;
        public float _hp;
        protected MeleeAttackable _attack;
        public LayerMask WallLayer;
        public Transform WallCheck;
        protected Transform startingPosition;
        protected Vector2 patrolDirection;
        protected PlayerFinder playerFinder;
        protected EnemyAttack attackSense;

        // HP UI
        protected UI_UnitHP _unitHPUI = null;

        /*
        private void Start()
        {
            _characterController2D = GetComponent<CharacterController2D>();
            rigid = GetComponent<Rigidbody2D>();
            _startingPosition = transform.position;
            _player = GameObject.Find("Player");
            //state = State.Patrol;
            state = State.Attack;

            _damage = new UnitHP(10);
            CreateHpUI();

            _patorlDirection = new Vector2(_speed, 0f);
            _attack.Init(1,1);
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

                case State.AttackStandby:
                    StartCoroutine(AttackCoolDown());
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

            }

            _characterController2D.OnFixedUpdate();
            _hp = _damage.GetHP();
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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
                state = State.Hitted;
        }

        private IEnumerator AttackEnd()
        {
            yield return new WaitForSeconds(0.7f);
            isAttack = false;
            state = State.AttackStandby;
        }

        private IEnumerator AttackCoolDown()
        {
            yield return new WaitForSeconds(_attackCoolTime);
            state = State.Attack;
        } */

        protected void CreateHpUI()
        {
            Debug.LogWarning("??");
            // UI매니저로 UI_UnitHP 생성
            _unitHPUI = UIManager.Instance.ShowWorldSpaceUI<UI_UnitHP>();
            // UI에서 UnitHP 참조
            _unitHPUI.HP = HP;
            // 유닛 움직이면 같이 움직이도록 Parent 설정
            _unitHPUI.transform.SetParent(transform);
            // UI에서 이 오브젝트의 정보가 필요할 수도 있으므로 참조
            _unitHPUI.Owner = gameObject;
            // 오브젝트의 머리 위에 위치하도록 설정
            _unitHPUI.SetPosition(transform.position, Vector3.up * 2);
        }

        protected void Flip()
        {
            Vector2 thisScale = transform.localScale;
            if (patrolDirection.x >= 0)
            {
                thisScale.x = -Mathf.Abs(thisScale.x);
                patrolDirection = new Vector2(-_speed, 0f);
                _sprite.flipX = false;
            }
            else
            {
                thisScale.x = Mathf.Abs(thisScale.x);
                patrolDirection = new Vector2(_speed, 0f);
                _sprite.flipX = true;
            }
            transform.localScale = thisScale;
            _unitHPUI.Reverse();
        }
    }
}