using System;
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

        public void Init(int x = 1, int y = 1)
        {
            this.x = x;
            this.y = y;
            size = new Vector2(x, y);
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
            if (_isFlip) Gizmos.DrawWireCube(new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y , gameObject.transform.position.z), new Vector3(x,y,0));
            else Gizmos.DrawWireCube(new Vector3(gameObject.transform.position.x -1, gameObject.transform.position.y , gameObject.transform.position.z), new Vector3(x,y,0));

        }

        public void FixedUpdate()
        {
            if (!_isParing) return;

            int facing = -1;
            if (_sprite.flipX != _isFlip) facing = 1;
            Vector2 pointA = new Vector2(transform.position.x + facing, transform.position.y);
            int hitCount = Physics2D.OverlapArea(pointA, size, _filter, _result);
            for (int i = 0; i < hitCount; i++)
            {
                _hitObj = _result[i];
                Debug.Log(_hitObj.tag);
                if (_hitObj.tag.Equals("Enemy"))
                {
                    if (_hitObj.GetComponent<BatMonster>().isAttack())
                    {
                        gameObject.GetComponent<PlayerCharacter>().PlayerInvincibility();
                        Debug.Log("패링 성공!");
                    }
                }
                else if (_hitObj.gameObject.layer == 10) //10: projectile
                {
                    gameObject.GetComponent<PlayerCharacter>().PlayerInvincibility();
                    _hitObj.GetComponent<Projectile>().Paring();
                    Debug.Log("패링 성공!");
                }
            }
        }
    }
}