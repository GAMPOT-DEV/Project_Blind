using System.Collections;
using UnityEngine;

namespace Blind
{
    public class ShamanMonster : CrowdEnemyCharacter
    {
        public GameObject Circle;

        private Coroutine Co_attack;
        private Coroutine Co_stun;
        private Coroutine Co_die;
        private Coroutine Co_avoid;

        private State tmp = State.Die;

        [SerializeField] private float _projectileSpeed = 10;

        protected void Awake()
        {
            base.Awake();
            /*
            sensingRange = new Vector2(8f, 5f);
            _speed = 0.07f;
            _runSpeed = 0.1f;
            _attackCoolTime = 0.5f;
            _attackSpeed = 0.3f;
            _attackRange = new Vector2(6f, 5f);
            _stunTime = 1f;
            */
            _patrolTime = 1.5f;
        }

        private void Start()
        {
            //startingPosition = gameObject.transform;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if(state != tmp)
            {
                Debug.Log(state);
                tmp = state;
            }
        }

        protected override void updateAttack()
        {
            _anim.SetBool("Basic Attack", true);
        }

        protected override void updateAvoid()
        {
            if (Co_avoid == null)
            {
                _anim.SetBool("Avoid", true);
                Co_avoid = StartCoroutine(CoAvoid());
            }

            if (Physics2D.OverlapCircle(WallCheck.position, 0.01f, WallLayer))
            {
                StopCoroutine(Co_avoid);
                Flip();
                state = State.Attack;
            }

            _characterController2D.Move(-playerFinder.ChasePlayer() * Data.runSpeed);
        }

        protected override void updateStun()
        {
            StopAllCoroutines();
            Co_stun = StartCoroutine(CoStun());
        }

        public void AniMakeProjectile()
        {
            var projectile = Instantiate(Circle, WallCheck.position, transform.rotation);
            Vector2 dir = playerFinder.PlayerPosition().position - gameObject.transform.position;
            projectile.GetComponent<Projectile>().SetProjectile(dir, Data.damage, _projectileSpeed);
            _anim.SetBool("Basic Attack", false);
            NextAction();
        }

        private IEnumerator CoAvoid()
        {
            Flip();
            yield return new WaitForSeconds(1);
            Flip();

            if (attackSense.Attackable())
                state = State.Attack;
            else if (playerFinder.FindPlayer())
                state = State.Chase;
            else
                state = State.Default;

            Co_avoid = null;
            _anim.SetBool("Avoid", false);
        }

        protected override void NextAction()
        {
            if (attackSense.Attackable())
            {
                if (attackSense.isAvoid())
                    state = State.Avoid;
                else
                    state = State.Attack;
            }
            else if (playerFinder.FindPlayer())
            {
                state = State.Chase;
            }
            else
            {
                state = State.Default;
            }
        }
    }
}