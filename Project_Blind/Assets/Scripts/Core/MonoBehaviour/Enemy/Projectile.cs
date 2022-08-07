using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind {
    public class Projectile : MonoBehaviour
    {
        private int _damage = 1;
        private Vector2 dir;

        public void SetProjectile(Vector2 dir, int damage)
        {
            this.dir = dir;
            gameObject.GetComponent<Rigidbody2D>().velocity = this.dir;

            Destroy(gameObject, 5);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name == "Player")
            {
                Blind.PlayerCharacter player = collision.gameObject.GetComponent<Blind.PlayerCharacter>();
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

        private void rotate()
        {
            Vector2 thisScale = transform.localScale;
            
            if (dir.x < 0)
            {
                thisScale.x = -Mathf.Abs(thisScale.x);
                transform.localScale = thisScale;
            }
        }
    }
}