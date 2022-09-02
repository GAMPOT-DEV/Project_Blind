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
 
         private Vector2 hitbox;
 
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
             Gizmos.DrawLine(hitbox, new Vector2(size.x, hitbox.y));
             Gizmos.DrawLine(hitbox, new Vector2(hitbox.x, size.y));
             Gizmos.DrawLine(new Vector2(size.x, hitbox.y), size);
             Gizmos.DrawLine(size, new Vector2(hitbox.x, size.y));
         }
 
         private void MeleeAttack()
         {
             var _player = gameObject.GetComponent<PlayerCharacter>();
             var facing = Facing.Left;
             if (sprite == null)
             {
                 if (_skeletonComponent.Skeleton.FlipX) facing = Facing.Right;
             }
             else
             {
                 if (sprite.flipX) facing = Facing.Right;
             }
 
             var pointA = new Vector2(_player._playerposition.x + (float) facing * 1, _player._playerposition.y + 2);
             hitbox = pointA;
             if (facing == Facing.Right) size = new Vector2(pointA.x + x, pointA.y - y);
             else size = new Vector2(pointA.x - x, pointA.y - y);
             var hitCount = Physics2D.OverlapArea(pointA, size, _attackcontactfilter, ResultObj);
             for (var i = 0; i < hitCount; i++)
             {
                 hitobj = ResultObj[i];
                 Debug.Log(hitobj.name);
                 if (hitobj.tag.Equals("Enemy"))
                 {
                     hitobj.GetComponent<EnemyCharacter>().HittedWithKnockBack(new AttackInfo(_damage, facing));
                     if (_player.CurrentWaveGauge + _player.attackWaveGauge < _player.maxWaveGauge)
                         _player.CurrentWaveGauge += _player.attackWaveGauge;
                     else _player.CurrentWaveGauge = _player.maxWaveGauge;
                     canDamage = false;
                     Debug.Log("맞음");
                 }
             }
         }
 
         private void EnemyMeleeAttack()
         {
             var gameobject = gameObject.GetComponent<EnemyCharacter>();
             Facing facing = Facing.Left;
             if (sprite.flipX != _isSpriteFlip) facing = Facing.Right;
 
             var pointA = new Vector2(transform.position.x + (float)facing * 1, transform.position.y);
             if (facing == Facing.Right) size = new Vector2(pointA.x + x, pointA.y - y);
             else size = new Vector2(pointA.x - x, pointA.y - y);
 
             var hitCount = Physics2D.OverlapArea(pointA, size, _attackcontactfilter, ResultObj);
             for (var i = 0; i < hitCount; i++)
             {
                 hitobj = ResultObj[i];
                 if (hitobj.tag.Equals("Player"))
                 {
                     var _player = hitobj.GetComponent<Character>();
                     _player.HittedWithKnockBack(new AttackInfo(_damage, facing));
                 }
                 canDamage = false;
             }
         }
         public void FixedUpdate()
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
     