using System.Collections;
using UnityEngine;

namespace Blind
{
    public class ShamanMonster : CrowdEnemyCharacter
    {
        public GameObject Circle;

        private Coroutine Co_default;
        private Coroutine Co_attack;
        private Coroutine Co_attackStandby;
        private Coroutine Co_stun;
        private Coroutine Co_die;
        private Coroutine Co_avoid;

        [SerializeField] private float _projectileSpeed = 10;

        protected void Awake()
        {
            base.Awake();
            _sensingRange = new Vector2(8f, 5f);
            _speed = 0.07f;
            _runSpeed = 0.1f;
            _attackCoolTime = 0.5f;
            _attackSpeed = 0.3f;
            _attackRange = new Vector2(6f, 5f);
            _stunTime = 1f;
        }

        private void Start()
        {
            startingPosition = gameObject.transform;
            player = GameObject.Find("Player");
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        protected override void updatePatrol()
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

        protected override void updateDefault()
        {
            if (Co_default == null)
                Co_default = StartCoroutine(CoWaitDefalut(1f));

            if (playerFinder.FindPlayer())
            {
                StopCoroutine(Co_default);
                Co_default = null;
                state = State.Chase;
            }
        }

        protected override void updateChase()
        {
            if (playerFinder.MissPlayer())
            {
                state = State.Patrol;
                return;
            }

            if (attackSense.Attackable())
            {
                //if (attackSense.isAvoid())
                //state = State.Avoid;
                //else
                state = State.AttackStandby;
                return;
            }

            _characterController2D.Move(playerFinder.ChasePlayer() * _runSpeed);
        }

        protected override void updateAttack()
        {
            if (Co_attack == null) Co_attack = StartCoroutine(CoAttack());
        }

        protected override void updateAttackStandby()
        {
            if (Co_attackStandby == null) Co_attackStandby = StartCoroutine(CoAttackStandby(_attackSpeed));
        }

        protected override void updateHitted()
        {
            var hittedVelocity = Vector2.zero;
            if (playerFinder.ChasePlayer().x > 0) //플레이어가 오른쪽
                hittedVelocity = new Vector2(-200, 400);
            else
                hittedVelocity = new Vector2(200, 400);

            rigid.AddForce(hittedVelocity);

            if (Hp.GetHP() <= 0)
                state = State.Die;
            else if (attackSense.Attackable())
                state = State.AttackStandby;
            else if (playerFinder.FindPlayer())
                state = State.Chase;
            else
                state = State.Patrol;
        }

        protected override void updateAvoid()
        {
            _characterController2D.Move(-playerFinder.ChasePlayer() * _runSpeed);
            if (Co_avoid == null) Co_avoid = StartCoroutine(CoAvoid());

            if (Physics2D.OverlapCircle(WallCheck.position, 0.01f, WallLayer))
            {
                StopCoroutine(Co_avoid);
                Flip();
                state = State.AttackStandby;
            }
        }

        protected override void updateStun()
        {
            StopAllCoroutines();
            Co_stun = StartCoroutine(CoStun());
        }

        protected override void updateDie()
        {
            Co_die = StartCoroutine(CoDie());
        }

        public bool isAttack()
        {
            if (state == State.Attack)
                return true;
            return false;
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
            var projectile = Instantiate(Circle, WallCheck.position, transform.rotation);
            Vector2 dir = playerFinder.PlayerPosition().position - gameObject.transform.position;
            projectile.GetComponent<Projectile>().SetProjectile(dir, _damage, _projectileSpeed);

            yield return new WaitForSeconds(2f);
            yield return new WaitForSeconds(0.2f);

            _attack.DisableDamage();

            if (attackSense.Attackable())
            {
                if (attackSense.isAvoid())
                    state = State.Avoid;
                else
                    state = State.AttackStandby;
            }
            else if (playerFinder.FindPlayer())
            {
                state = State.Chase;
            }
            else
            {
                state = State.Default;
            }

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
                color.a -= .25f * Time.deltaTime;

                _sprite.color = color;
                //wait for a frame
                yield return null;
            }

            Destroy(gameObject);
        }

        private IEnumerator CoAvoid()
        {
            Flip();
            yield return new WaitForSeconds(1);
            Flip();
            if (attackSense.Attackable())
                state = State.AttackStandby;
            else if (playerFinder.FindPlayer())
                state = State.Chase;
            else
                state = State.Default;
            Co_avoid = null;
        }
    }
}