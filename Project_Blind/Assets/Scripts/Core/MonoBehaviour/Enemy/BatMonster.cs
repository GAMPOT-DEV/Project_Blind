using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    /// <summary>
    /// �ٰŸ� ���Ϳ� ���� Ŭ�����Դϴ�.
    /// </summary>
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
            Stun,
            Die
        }

        private CharacterController2D _characterController2D;
        private Rigidbody2D rigid;
        private SpriteRenderer _sprite;




        [SerializeField] private Vector2 _sensingRange;
        [SerializeField] private float _speed;
        [SerializeField] private float _runSpeed;
        [SerializeField] private float _attackCoolTime;
        [SerializeField] private float _attackSpeed;
        [SerializeField] private Vector2 _attackRange;
        [SerializeField] private int _maxHP;
        [SerializeField] private int _damage;
        [SerializeField] private float _stunTime;
        

        private State state;
        private GameObject player;
        RaycastHit2D[] rayHit;
        public UnitHP HP;
        public float _hp;
        private MeleeAttackable _attack;
        public LayerMask WallLayer;
        public Transform WallCheck;
        private Transform startingPosition;
        private Vector2 patrolDirection;
        private PlayerFinder playerFinder;
        private EnemyAttack attackSense;

        private Coroutine Co_default;
        private Coroutine Co_attack;
        private Coroutine Co_attackStandby;
        private Coroutine Co_stun;

        // HP UI
        private UI_UnitHP _unitHPUI = null;
        private void Awake()
        {
            //�� ������ �ʱ�ȭ�ϸ� ������ �� ����...? ���� ã�Ƽ� �� �� ������ �� ��
            _sensingRange = new Vector2(10f, 5f);
            _attack = GetComponent<MeleeAttackable>();
            _sprite = GetComponent<SpriteRenderer>();
            _speed = 0.1f;
            _runSpeed = 0.2f;
            _attackCoolTime = 0.5f;
            _attackSpeed = 0.3f;
            _attackRange = new Vector2(1.5f, 2f);
            _maxHP = 10;
            _stunTime = 1f;
            _characterController2D = GetComponent<CharacterController2D>();
            rigid = GetComponent<Rigidbody2D>();
            state = State.Patrol;

            HP = new UnitHP(_maxHP);
            // HP UI 생성
            CreateHpUI();

            patrolDirection = new Vector2(RandomDirection() * _speed, 0f);
            _attack.Init(2, 2);

            playerFinder = GetComponentInChildren<PlayerFinder>();
            playerFinder.setRange(_sensingRange);
            attackSense = GetComponentInChildren<EnemyAttack>();
            attackSense.setRange(_attackRange);
        }

        private void Start()
        {
            startingPosition = gameObject.transform;
            player = GameObject.Find("Player");

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

                case State.Stun:
                    updateStun();
                    break;

                case State.Die:
                    updateDie();
                    break;
            }
            _characterController2D.OnFixedUpdate();
            _hp = HP.GetHP();
        }

        private void updatePatrol()
        {
            if (playerFinder.FindPlayer())
            {
                state = State.Chase;
                return;
            }
            if (Physics2D.OverlapCircle(WallCheck.position, 0.01f, WallLayer))
            {
                state = State.Default;
                return;
            }

            _characterController2D.Move(patrolDirection);
        }

        private void updateDefault()
        {
            if (Co_default == null)
                Co_default = StartCoroutine(CoWaitDefalut(1f));

            if (playerFinder.FindPlayer())
            {
                StopCoroutine(Co_default);
                Co_default = null;
                state = State.Chase;
                return;
            }
        }

        private void updateChase()
        {
            if (playerFinder.MissPlayer())
            {
                state = State.Patrol;
                return;
            }

            if (attackSense.Attackable())
            {
                state = State.AttackStandby;
                return;
            }

            _characterController2D.Move(playerFinder.ChasePlayer() * _runSpeed);
        }
        
        private void updateAttack()
        {
            if (Co_attack == null)
            {
                Co_attack = StartCoroutine(CoAttack());
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }

        private void updateAttackStandby()
        {
            if (Co_attackStandby == null)
            {
                Co_attackStandby = StartCoroutine(CoAttackStandby(_attackSpeed));
                gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
        }

        private void updateHitted()
        {

        }

        private void updateStun()
        {
            StopAllCoroutines();
            Co_stun = StartCoroutine(CoStun());
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

        private int RandomDirection()
        {
            int RanNum = Random.Range(0, 100);
            if (RanNum > 50)
                return 1;
            else
            {
                Flip();
                return -1;
            }
        }

        public bool ReturnFacing()
        {
            return _sprite.flipX;
        }
        private IEnumerator CoWaitDefalut(float time)
        {
            yield return new WaitForSeconds(time);
            Flip();
            state = State.Patrol;
            Co_default = null;
        }

        private IEnumerator CoAttackStandby(float time)
        {
            Debug.Log("���� �غ�");
            yield return new WaitForSeconds(time); //����
            Debug.Log("���� !");
            state = State.Attack;
            Co_attackStandby = null;
        }

        private IEnumerator CoAttack()
        {
            yield return new WaitForSeconds(2f); //����
            Debug.Log("공격!!");
            _attack.EnableDamage();
            gameObject.GetComponent<SpriteRenderer>().color = Color.blue; //000FE9 ���� ��
            yield return new WaitForSeconds(0.2f); //�ĵ�
            
            _attack.DisableDamage();
            if (attackSense.Attackable())
                state = State.AttackStandby;
            else if (playerFinder.FindPlayer())
                state = State.Chase;
            else
                state = State.Default;
            Co_attack = null;
        }

        private IEnumerator CoStun()
        {
            yield return new WaitForSeconds(_stunTime);

            if (attackSense.Attackable())
                state = State.AttackStandby;
            else if (playerFinder.FindPlayer())
                state = State.Chase;
            else
                state = State.Patrol;

            Co_stun = null;
        }

        private void CreateHpUI()
        {
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
    }
}