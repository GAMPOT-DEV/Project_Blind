using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind {
    public class Projectile : MonoBehaviour
    {
        private int _damage = 1;
        private Vector2 dir;
        private bool isParing = false;

        public void SetProjectile(Vector2 dir, int damage)
        {
            this.dir = dir;
            gameObject.GetComponent<Rigidbody2D>().velocity = this.dir;
            StartCoroutine(CoDestroy());
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (isParing)
            {
                if (collision.CompareTag("Enemy"))
                {
                    EnemyCharacter enemy = collision.gameObject.GetComponent<EnemyCharacter>();
                    enemy.HP.GetDamage(_damage);
                }
                else if (collision.gameObject.layer == 6)
                {
                    Destroy(gameObject);
                }
            }
            else {
                if (collision.name == "Player")
                {
                    PlayerCharacter player = collision.gameObject.GetComponent<PlayerCharacter>();
                    player._damage.GetDamage(_damage);
                    player.OnHurt();
                    int facing = dir.x >= 0 ? 1 : -1;
                    player.HurtMove(player._hurtMove * facing);

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