using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class ShamanMonster : EnemyCharacter
    {
        private enum State
        {
            Patrol,
            Default,
            Chase,
            Attack,
            AttackStandby,
            Hitted,
            Avoid,
            Stun,
            Die
        }

        private State state;
        public GameObject Circle;

        private Coroutine Co_default;
        private Coroutine Co_attack;
        private Coroutine Co_attackStandby;
        private Coroutine Co_stun;
        private Coroutine Co_die;
        private Coroutine Co_avoid;

        private void Awake()
        {
            _sensingRange = new Vector2(16f, 15f);
            _attack = GetComponent<MeleeAttackable>();
            _sprite = GetComponent<SpriteRenderer>();
            _speed = 0.1f;
            _runSpeed = 0.2f;
            _attackCoolTime = 0.5f;
            _attackSpeed = 0.3f;
            _attackRange = new Vector2(14f, 15f);
            _maxHP = 10;
            _stunTime = 1f;
            _characterController2D = GetComponent<CharacterController2D>();
            rigid = GetComponent<Rigidbody2D>();
            state = State.Patrol;
            HP = new UnitHP(_maxHP);
            CreateHpUI();
            patrolDirection = new Vector2(RandomDirection() * _speed, 0f); //버그 고친 후 수정할 것
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

                case State.Avoid:
                    updateAvoid();
                    break;

                case State.Stun:
                    updateStun();
                    break;

                case State.Die:
                    updateDie();
                    break;
            }
            //움직임
            _characterController2D.OnFixedUpdate();
            //체력 업데이트
            if (_hp < HP.GetHP())
                //state = State.Hitted;
            _hp = HP.GetHP();
            Debug.Log(state);
        }

        private void updatePatrol()
        {
            if (playerFinder.FindPlayer())
            {
                if (playerFinder.AvoidOrChase() < 0)
                {
                    state = State.Chase;
                    return;
                }
                else
                {
                    state = State.Avoid;
                    return;
                }
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
            Vector2 hittedVelocity = Vector2.zero;
            if (playerFinder.ChasePlayer().x > 0) //플레이어가 오른쪽
            {
                hittedVelocity = new Vector2(-200, 400);
            }
            else
            {
                hittedVelocity = new Vector2(200, 400);
            }

            rigid.AddForce(hittedVelocity);

            if (_hp <= 0)
                state = State.Die;
            else if (attackSense.Attackable())
                state = State.AttackStandby;
            else if (playerFinder.FindPlayer())
                state = State.Chase;
            else
                state = State.Patrol;
        }

        private void updateAvoid()
        {
            _characterController2D.Move(-playerFinder.ChasePlayer() * _runSpeed);

            if (Co_avoid == null)
                Co_avoid = StartCoroutine(CoAvoid());

            if (Physics2D.OverlapCircle(WallCheck.position, 0.01f, WallLayer))
            {
                Flip();
            }
        }

        private void updateStun()
        {
            StopAllCoroutines();
            Co_stun = StartCoroutine(CoStun());
        }

        private void updateDie()
        {
            Co_die = StartCoroutine(CoDie());
        }

        public bool isAttack()
        {
            if (state == State.Attack)
                return true;
            else
                return false;
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

        private IEnumerator CoWaitDefalut(float time)
        {
            yield return new WaitForSeconds(time);
            Flip();
            state = State.Patrol;
            Co_default = null;
        }

        private IEnumerator CoAttackStandby(float time)
        {
            yield return new WaitForSeconds(time);
            state = State.Attack;
            Co_attackStandby = null;
        }

        private IEnumerator CoAttack()
        {
            GameObject projectile = Instantiate(Circle, WallCheck.position, transform.rotation);
            projectile.transform.SetParent(transform);
            Vector2 dir = playerFinder.PlayerPosition().position - gameObject.transform.position;
            projectile.GetComponent<Projectile>().SetProjectile(dir, _damage);

            yield return new WaitForSeconds(2f);
            gameObject.GetComponent<SpriteRenderer>().color = Color.blue; //000FE9
            yield return new WaitForSeconds(0.2f);

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

        private IEnumerator CoDie()
        {
            yield return new WaitForSeconds(1);
            while (_sprite.color.a > 0)
            {
                var color = _sprite.color;
                //color.a is 0 to 1. So .5*time.deltaTime will take 2 seconds to fade out
                color.a -= (.25f * Time.deltaTime);

                _sprite.color = color;
                //wait for a frame
                yield return null;
            }
            Destroy(gameObject);
        }

        private IEnumerator CoAvoid()
        {
            yield return new WaitForSeconds(3);
            if (attackSense.Attackable())
                state = State.Attack;
            else if (playerFinder.FindPlayer() && playerFinder.AvoidOrChase() < 0)
                state = State.Chase;
            else
                state = State.Patrol;
        }
    }
}