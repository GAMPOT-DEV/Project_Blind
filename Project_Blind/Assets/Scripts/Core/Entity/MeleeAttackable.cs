using Spine.Unity;
using UnityEngine;
     
 namespace Blind
 {
     public class MeleeAttackable : MonoBehaviour
     {
         private int x;
         private int y;
         private float _damage = 1;
         private int _defultdamage;
         private bool canDamage;
         private Vector2 size;
         private SpriteRenderer sprite;
         private ISkeletonComponent _skeletonComponent;
         private readonly Collider2D[] ResultObj = new Collider2D[10];
         private ContactFilter2D _attackcontactfilter;
         public LayerMask hitLayer;
         private Collider2D hitobj;
         private bool _isSpriteFlip;
 
         private Vector2 _hitbox;
 
         public void Awake()
         {
             _attackcontactfilter.layerMask = hitLayer;
             _attackcontactfilter.useLayerMask = true;
             sprite = GetComponent<SpriteRenderer>();
             _skeletonComponent = GetComponent<SkeletonMecanim>();
             if (sprite != null) _isSpriteFlip = sprite.flipX;
         }
 
         public void Init(int x = 1, int y = 1, int _damage = 1)
         {
             this.x = x;
             this.y = y;
             this._damage = _damage;
             _defultdamage = _damage;
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
             Gizmos.DrawLine(_hitbox, new Vector2(size.x, _hitbox.y));
             Gizmos.DrawLine(_hitbox, new Vector2(_hitbox.x, size.y));
             Gizmos.DrawLine(new Vector2(size.x, _hitbox.y), size);
             Gizmos.DrawLine(size, new Vector2(_hitbox.x, size.y));
         }
 
         private void MeleeAttack()
         {
             var entity = gameObject.GetComponent<Character>();
             var facing = entity.GetFacing();
             
             var position = transform.position;
             var pointA = new Vector2(position.x + (float)facing * 1, position.y + 1);
             
             _hitbox = pointA;
             size = new Vector2(pointA.x + ((float)facing * x), pointA.y + y);
             
             var hitCount = Physics2D.OverlapArea(pointA, size, _attackcontactfilter, ResultObj);
             for (var i = 0; i < hitCount; i++)
             {
                 hitobj = ResultObj[i];
                 hitobj.GetComponent<Character>().HitWithKnockBack(new AttackInfo(_damage, facing));
                 entity.HitSuccess();
                 canDamage = false;
             }
         }
         public void FixedUpdate()
         {
             if (!canDamage) return;
             MeleeAttack();
         }
     }
 }
     