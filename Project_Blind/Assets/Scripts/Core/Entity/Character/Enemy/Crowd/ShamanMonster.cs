using System.Collections;
using UnityEngine;

namespace Blind
{
    public class ShamanMonster : CrowdEnemyCharacter
    {
        public GameObject Circle;
        private Coroutine Co_avoid;

        protected void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        protected override void updateAttack()
        {
            flipToFacing();
            _anim.SetInteger("State", 3);
        }

        protected override void updateAvoid()
        {
            if (Co_avoid == null)
            {
                
                Co_avoid = StartCoroutine(CoAvoid());
            }

            if (Physics2D.OverlapCircle(WallCheck.position, 0.01f, WallLayer))
            {
                StopCoroutine(Co_avoid);
                Flip();
                state = State.Attack;
            }

            _anim.SetInteger("State", 7);
            _characterController2D.Move(-playerFinder.ChasePlayer() * Data.runSpeed);
        }

        public void AniMakeProjectile()
        {
            var projectile = Instantiate(Circle, WallCheck.position, transform.rotation);
            Vector2 dir = player.transform.position - gameObject.transform.position;
            projectile.GetComponent<Projectile>().SetProjectile(dir, Data.damage, Data.attackSpeed, gameObject);
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
                state = State.Patrol;

            Co_avoid = null;
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

        public void DeadSound()
        {
            SoundManager.Instance.Play("Crowd/Shaman/Death");
        }

        public void AttackSound()
        {
            SoundManager.Instance.Play("Crowd/Shaman/BigAttack");
        }

        public override void WalkSound()
        {
            SoundManager.Instance.Play("Crowd/Shaman/Move");
        }
    }
}