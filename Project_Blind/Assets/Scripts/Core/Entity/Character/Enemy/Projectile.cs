using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind {
    public class Projectile : MonoBehaviour
    {
        private int _damage = 1;
        private Vector2 dir;
        private bool isParing = false;

        public void SetProjectile(Vector2 dir, int damage, float speed)
        {
            this.dir = dir;
            gameObject.GetComponent<Rigidbody2D>().velocity = this.dir.normalized * speed;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.Rotate(new Vector3(0, 0, angle));
            StartCoroutine(CoDestroy());
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (isParing)
            {
                if (collision.CompareTag("Enemy"))
                {
                    EnemyCharacter enemy = collision.gameObject.GetComponent<EnemyCharacter>();
                    enemy.Hp.GetDamage(_damage);
                }
                else if (collision.gameObject.layer == 6)
                {
                    Destroy(gameObject);
                }
            }
            else {
                if (collision.tag.Equals("Player"))
                {
                    PlayerCharacter player = collision.gameObject.GetComponent<PlayerCharacter>();
                    Facing facing = dir.x >= 0 ? Facing.Right : Facing.Left;
                    player.HittedWithKnockBack(new AttackInfo(_damage,facing));

                    Destroy(gameObject);
                }
                else if (collision.gameObject.layer == 6)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void rotate()
        {
            Vector2 thisScale = transform.localScale;
            
            if (dir.x < 0)
            {
                thisScale.x = -Mathf.Abs(thisScale.x);
                transform.localScale = thisScale;
            }
        }

        public void Paring()
        {
            isParing = true;
            StopCoroutine(CoDestroy());
            gameObject.GetComponent<Rigidbody2D>().velocity
                = GetComponentInParent<Transform>().position;
            StartCoroutine(CoDestroy());
        }

        private IEnumerator CoDestroy()
        {
            yield return new WaitForSeconds(5f);
            Destroy(gameObject);
        }
    }
}