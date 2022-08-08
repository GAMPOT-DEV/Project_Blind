using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class EnemyCharacter : MonoBehaviour
    {
        protected enum State
        {
            Patrol,
            Default,
            Chase,
            Attack,
            AttackStandby,
            Hitted,
            Stun,
            Avoid,
            Die
        }

        protected State state;
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

        protected void Init()
        {
            patrolDirection = new Vector2(RandomDirection() * _speed, 0f);
            _attack = GetComponent<MeleeAttackable>();
            _sprite = GetComponent<SpriteRenderer>();
            _characterController2D = GetComponent<CharacterController2D>();
            rigid = GetComponent<Rigidbody2D>();
            HP = new UnitHP(_maxHP);
            CreateHpUI();
            _hp = HP.GetHP();
            playerFinder = GetComponentInChildren<PlayerFinder>();
            playerFinder.setRange(_sensingRange);
            attackSense = GetComponentInChildren<EnemyAttack>();
            attackSense.setRange(_attackRange);
            state = State.Patrol;
        }

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

        protected int RandomDirection()
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
    }
}