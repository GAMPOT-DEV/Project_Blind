using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind {
    public class Projectile : MonoBehaviour
    {
        private int _damage = 1;
        private Vector2 dir;
        private bool isParing = false;
        private GameObject monster;
        private float speed;

        public void SetProjectile(Vector2 dir, int damage, float speed, GameObject shaman)
        {
            this.dir = dir;
            this.speed = speed;
            monster = shaman;
            gameObject.GetComponent<Rigidbody2D>().velocity = this.dir.normalized * speed;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.Rotate(new Vector3(0, 0, angle));
            StartCoroutine(CoDestroy());
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (isParing)
            {
                if (collision.CompareTag("Enemy") && collision.gameObject.layer != 16)
                {
                    CrowdEnemyCharacter enemy = collision.gameObject.GetComponent<CrowdEnemyCharacter>();
                    enemy.OnStun();

                    Destroy(gameObject);
                }
            }
            else {
                if (collision.tag.Equals("Player"))
                {
                    PlayerCharacter player = collision.gameObject.GetComponent<PlayerCharacter>();
                    Facing facing = dir.x >= 0 ? Facing.Right : Facing.Left;
                    player.HitWithKnockBack(new AttackInfo(_damage,facing));

                    Destroy(gameObject);
                }
            }
        }

        public void OnParing()
        {
            isParing = true;
            StopCoroutine(CoDestroy());
            Vector2 dir = monster.transform.position - gameObject.transform.position + new Vector3(0, 2f, 0);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.Rotate(new Vector3(0, 0, angle));

            gameObject.GetComponent<Rigidbody2D>().velocity = dir.normalized * speed;
            StartCoroutine(CoDestroy());
        }

        private IEnumerator CoDestroy()
        {
            yield return new WaitForSeconds(5f);
            Destroy(gameObject);
        }
    }
}