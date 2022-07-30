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
            Debug.Log("실행됨!!");
            canDamage = true;
        }

        public void DisableDamage()
        {
            canDamage = false;
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
            int facing = 1;
            Debug.Log(sprite + "test");
            if (sprite.flipX != _isSpriteFlip)
            {
                facing = -1;
            }

            Vector2 pointA = new Vector2(transform.position.x + facing, transform.position.y);
            int hitCount = Physics2D.OverlapArea(pointA, size, _attackcontactfilter, ResultObj);
            for (int i = 0; i < hitCount; i++)
            {
                hitobj = ResultObj[i];
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
            int facing = -1;
            Debug.Log(sprite);
            if (sprite.flipX != _isSpriteFlip)
            {
                facing = 1;
            }

            Vector2 pointA = new Vector2(transform.position.x + facing, transform.position.y);
            int hitCount = Physics2D.OverlapArea(pointA, size, _attackcontactfilter, ResultObj);
            for (int i = 0; i < hitCount; i++)
            {
                hitobj = ResultObj[i];
                Debug.Log(hitobj.tag);
                if (hitobj.tag.Equals("Player"))
                {
                    Debug.Log("dd");
                    hitobj.GetComponent<PlayerCharacter>()._damage.GetDamage(_damage);
                    hitobj.GetComponent<PlayerCharacter>().OnHurt();
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
                if(!canDamage) return;
                
                EnemyMeleeAttack();
            }
            
            
        }
    }
}