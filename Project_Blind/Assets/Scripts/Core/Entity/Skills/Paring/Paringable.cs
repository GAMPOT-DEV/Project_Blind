using System;
using System.Collections;
using System.Security.Cryptography;
using Cinemachine;
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
        [SerializeField] private GameObject ParingBox;
        private BoxCollider2D _paringBox;
        private Facing face = Facing.Left;
        public void Init(int x = 1, int y = 1)
        {
            this.x = x;
            this.y = y;
            _sprite = GetComponent<SpriteRenderer>();
            _skeletonComponent = GetComponent<SkeletonMecanim>();
            _paringBox = ParingBox.GetComponent<BoxCollider2D>();
            _filter.layerMask = _hitLayer;
            _filter.useLayerMask = true;
            if (_sprite != null)
            {
                _isFlip = _sprite.flipX;
            }
            else
            {
                _isFlip = _skeletonComponent.Skeleton.ScaleX < 0;
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
        public virtual void Facings(Facing _face)
        {
            if (face != _face)
            {
                face = _face;
                var position = transform.localPosition;
                position = new Vector3((float)_face*Math.Abs(position.x),position.y,position.z);
                transform.localPosition = position;
                var localScale = transform.localScale;
                localScale = new Vector3(-(float)_face*Math.Abs(localScale.x),localScale.y,localScale.z);
                transform.localScale = localScale;
            }
        }
        public void OnTriggerStay2D(Collider2D col)
        {
            if (!_isParing) return;
            Debug.Log("dd");
            Facings(gameObject.transform.parent.gameObject.GetComponent<PlayerCharacter>().GetFacing());
            var paring = col.gameObject.GetComponent<ParingEffect>();
            if (paring != null)
            {
                paring.GetParing();
                _isParing = false;
                var player = GameManager.Instance.Player;
                player.transform.GetChild(1).GetChild(9).GetComponent<AttackFX>().Play(player.GetComponent<PlayerCharacter>().GetFacing());
            }
        }
    }
}