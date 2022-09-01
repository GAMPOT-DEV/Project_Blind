using System;
using System.Security.Cryptography;
using Spine.Unity;
using UnityEngine;

namespace Blind
{
    public class Paringable: MonoBehaviour
    {
        private int x;
        private int y;
        private Vector2 size;
        private SpriteRenderer _sprite;
        private ISkeletonComponent _skeletonComponent;
        private Collider2D[] _result = new Collider2D[10];
        private ContactFilter2D _filter;
        private Collider2D _hitObj;
        private bool _isFlip = false;
        private bool _isParing;
        [SerializeField] private LayerMask _hitLayer;
        private Vector2 hitbox;

        public void Init(int x = 1, int y = 1)
        {
            this.x = x;
            this.y = y;
            _sprite = GetComponent<SpriteRenderer>();
            _skeletonComponent = GetComponent<SkeletonMecanim>();
            _filter.layerMask = _hitLayer;
            _filter.useLayerMask = true;
            if (_sprite != null)
            {
                _isFlip = _sprite.flipX;
            }
            else
            {
                _isFlip = _skeletonComponent.Skeleton.FlipX;
            }
        }

        public void EnParing()
        {
            _isParing = true;
        }

        public void DisableParing()
        {
            _isParing = false;
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(hitbox, new Vector2(size.x, hitbox.y));
            Gizmos.DrawLine(hitbox, new Vector2(hitbox.x, size.y));
            Gizmos.DrawLine(new Vector2(size.x,hitbox.y), size);
            Gizmos.DrawLine(size, new Vector2(hitbox.x, size.y));

        }

        public void FixedUpdate()
        {
            if (!_isParing) return;
            
            int facing = -1;
            if (_sprite == null)
            {
                if (_skeletonComponent.Skeleton.FlipX != _isFlip) facing = 1;
            }
            else
            {
                if (_sprite.flipX != _isFlip) facing = 1;
            }
            Vector2 pointA = new Vector2(gameObject.GetComponent<PlayerCharacter>()._playerposition.x + facing, gameObject.GetComponent<PlayerCharacter>()._playerposition.y);
            hitbox = pointA;
            if (facing == 1) size = new Vector2(pointA.x + x, pointA.y - y);
            else size = new Vector2(pointA.x - x, pointA.y - y);
            int hitCount = Physics2D.OverlapArea(pointA, size, _filter, _result);
            for (int i = 0; i < hitCount; i++)
            {
                _hitObj = _result[i];
                if (_hitObj.GetComponent<BatMonster>() != null)
                {
                    ParingEffect<BatMonster>.Initialise(_hitObj.GetComponent<BatMonster>());
                    BatMonsterParing batMonsterparing = _hitObj.gameObject.AddComponent<BatMonsterParing>();
                    batMonsterparing.OnCheckForParing(gameObject.GetComponent<PlayerCharacter>());
                    batMonsterparing.EnemyDibuff();
                    _isParing = false;
                    Destroy(batMonsterparing);
                }
                else if(_hitObj.GetComponent<BossHand>() != null)
                {
                    Debug.Log("sdw");
                    ParingEffect<BossHand>.Initialise(_hitObj.GetComponent<BossHand>());
                    BossHandParing bossHandParing = _hitObj.gameObject.AddComponent<BossHandParing>();
                    bossHandParing.OnCheckForParing(gameObject.GetComponent<PlayerCharacter>());
                    bossHandParing.EnemyDibuff();
                    _isParing = false;
                    Destroy(bossHandParing);
                }
            }
        }
    }
}