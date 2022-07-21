using System;
using UnityEngine;

namespace Blind
{
    public class MeleeAttackable: MonoBehaviour
    {
        private int x;
        private int y;
        private int _damage = 0;
        private bool canDamage;
        private Vector2 size;
        private SpriteRenderer sprite;
        private Collider2D[] ResultObj = new Collider2D[10];
        private ContactFilter2D _attackcontactfilter;
        public LayerMask hitLayer;
        private Collider2D hitobj;
        private bool _isSpriteFlip;
        public  void Init(int x = 1, int y = 1, int _damage = 1)
        {
            this.x = x;
            this.y = y;
            this._damage = _damage;
            size = new Vector2(x, y);
            _attackcontactfilter.layerMask = hitLayer;
            _attackcontactfilter.useLayerMask = true;
            sprite = GetComponent<SpriteRenderer>();
            if (sprite != null)
            {
                _isSpriteFlip = sprite.flipX;
            }
        }

        public void EnableDamage()
        {
            canDamage = true;
        }

        public void DisableDamage()
        {
            canDamage = false;
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            if (sprite.flipX) Gizmos.DrawWireCube(new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y , gameObject.transform.position.z), new Vector3(x,y,0));
            else Gizmos.DrawWireCube(new Vector3(gameObject.transform.position.x -1, gameObject.transform.position.y , gameObject.transform.position.z), new Vector3(x,y,0));

        }

        private void FixedUpdate()
        {
            if (!canDamage) return;
            
            int facing = 1;
            if (sprite.flipX != _isSpriteFlip)
            {
                facing = -1;
            }

            Vector2 pointA = new Vector2(transform.position.x + facing, transform.position.y);
            int hitCount = Physics2D.OverlapArea(pointA, size, _attackcontactfilter, ResultObj);
            for (int i = 0; i < hitCount; i++)
            {
                hitobj = ResultObj[i];
                if (hitobj.tag.Equals("Player"))
                {
                    hitobj.GetComponent<PlayerCharacter>()._damage.GetDamage(_damage);
                }
                else
                {
                    Debug.Log("맞음");
                    hitobj.GetComponent<EnemyCharacter>()._damage.GetDamage(_damage);
                }
            }
        }
    }
}