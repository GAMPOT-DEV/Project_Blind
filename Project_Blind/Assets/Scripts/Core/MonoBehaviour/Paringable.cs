using System;
using System.Security.Cryptography;
using UnityEngine;

namespace Blind
{
    public class Paringable: MonoBehaviour
    {
        private int x;
        private int y;
        private Vector2 size;
        private SpriteRenderer _sprite;
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
            _filter.layerMask = _hitLayer;
            _filter.useLayerMask = true;
            if (_sprite != null)
            {
                _isFlip = _sprite.flipX;
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
            if (_sprite.flipX != _isFlip) facing = 1;
            Vector2 pointA = new Vector2(transform.position.x + facing, transform.position.y);
            hitbox = pointA;
            if (facing == 1) size = new Vector2(pointA.x + x, pointA.y - y);
            else size = new Vector2(pointA.x - x, pointA.y - y);
            int hitCount = Physics2D.OverlapArea(pointA, size, _filter, _result);
            for (int i = 0; i < hitCount; i++)
            {
                _hitObj = _result[i];
                if (_hitObj.tag.Equals("Enemy"))
                {
                    if (_hitObj.GetComponent<BatMonster>().isAttack())
                    {
                        PlayerCharacter _player = gameObject.GetComponent<PlayerCharacter>();
                        _player.PlayerInvincibility();
                        if (_player.CurrentWaveGauge + _player.paringWaveGauge < _player.maxWaveGauge)
                            _player.CurrentWaveGauge += _player.paringWaveGauge;
                        else
                            _player.CurrentWaveGauge = _player.maxWaveGauge;
                        
                        _isParing = false;
                    }
                }
                else if (_hitObj.tag.Equals("Untagged")) //10: projectile
                {
                    gameObject.GetComponent<PlayerCharacter>().PlayerInvincibility();
                    _hitObj.GetComponent<Projectile>().Paring();
                    _isParing = false;
                }
            }
        }
    }
}