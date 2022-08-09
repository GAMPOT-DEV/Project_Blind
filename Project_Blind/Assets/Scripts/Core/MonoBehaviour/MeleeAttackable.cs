using System;
using UnityEngine;

namespace Blind
{
    public class MeleeAttackable: MonoBehaviour
    {
        private int x;
        private int y;
        private int _damage = 1;
        private int _defultdamage;
        private bool canDamage;
        private Vector2 size;
        private SpriteRenderer sprite = null;
        private Collider2D[] ResultObj = new Collider2D[10];
        private ContactFilter2D _attackcontactfilter;
        public LayerMask hitLayer;
        private Collider2D hitobj;
        private bool _isSpriteFlip = false;
        
        public void Awake()
        {
            _attackcontactfilter.layerMask = hitLayer;
            _attackcontactfilter.useLayerMask = true;
            sprite = GetComponent<SpriteRenderer>();
            if (sprite != null)
            {
                _isSpriteFlip = sprite.flipX;
            }
        }

        public  void Init(int x = 1, int y = 1, int _damage = 1)
        {
            this.x = x;
            this.y = y;
            this._damage = _damage;
            _defultdamage = _damage;
            size = new Vector2(x, y);
        }

        public void EnableDamage()
        {
            canDamage = true;
        }

        public void DisableDamage()
        {
            canDamage = false;
        }

        public void DamageReset(int damage)
        {
            _damage = damage;
        }

        public void DefultDamage()
        {
            _damage = _defultdamage;
        }

        public void AttackRangeReset(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        
        public void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            if (sprite.flipX) Gizmos.DrawWireCube(new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y , gameObject.transform.position.z), new Vector3(x,y,0));
            else Gizmos.DrawWireCube(new Vector3(gameObject.transform.position.x -1, gameObject.transform.position.y , gameObject.transform.position.z), new Vector3(x,y,0));

        }

        private void MeleeAttack()
        {
            int facing = -1;
            if (sprite.flipX)
            {
                facing = 1;
            }
            Vector2 pointA = new Vector2(transform.position.x + facing, transform.position.y);
            Vector2 pointB = new Vector2(pointA.x + x, pointA.y + y);
            int hitCount = Physics2D.OverlapArea(pointA, pointB, _attackcontactfilter, ResultObj);
            Debug.Log(hitCount);
            for (int i = 0; i < hitCount; i++)
            {
                hitobj = ResultObj[i];
                Debug.Log(hitobj.name);
                if(hitobj.tag.Equals("Enemy"))
                {
                    hitobj.GetComponent<BatMonster>().HP.GetDamage(_damage);
                    canDamage = false;
                    Debug.Log("맞음");
                }
            }
        }

        private void EnemyMeleeAttack()
        {
            BatMonster gameobject = gameObject.GetComponent<BatMonster>();
            int facing = -1;
            if (sprite.flipX != _isSpriteFlip)
            {
                facing = 1;
            }

            Vector2 pointA = new Vector2(transform.position.x + facing, transform.position.y);
            int hitCount = Physics2D.OverlapArea(pointA, size, _attackcontactfilter, ResultObj);
            for (int i = 0; i < hitCount; i++)
            {
                hitobj = ResultObj[i];
                if (hitobj.tag.Equals("Player"))
                {
                    PlayerCharacter _player = hitobj.GetComponent<PlayerCharacter>();
                    _player._damage.GetDamage(_damage);
                    _player.OnHurt();
                    _player.HurtMove(_player._hurtMove * _player.GetEnemyFacing(gameobject));
                    canDamage = false;
                }
            }
        }
        private void FixedUpdate()
        {
            if (gameObject.GetComponent<PlayerCharacter>() != null)
            {
                if (!canDamage) return;
                MeleeAttack();
            }
            else
            {
                if (!canDamage) return;
                
                EnemyMeleeAttack();
            }
            
            
        }
    }
}