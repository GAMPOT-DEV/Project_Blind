using System;
using UnityEngine;

namespace Blind
{
    /// <summary>
    /// 플레이어를 포함한 모든 캐릭터들의 움직임등을 처리하는 클래스입니다.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class CharacterController2D : MonoBehaviour,IGameManagerObj
    {
        private Rigidbody2D _rigidBody2D;
        private Collider2D _collider2D;

        private Vector2 _nextMovement;
        private Vector2 _previousPosition;
        private Vector2 _currentPosition;
        private int _groundMask;
        
        public Vector2 Velocity { get; private set; }
        public bool IsGrounded { get; protected set; }

        private void Awake()
        {
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _collider2D = GetComponent<Collider2D>();
            _previousPosition = _rigidBody2D.position;
            _currentPosition = _rigidBody2D.position;
            
            _groundMask = LayerMask.GetMask("Floor");
        }

        public void OnFixedUpdate()
        {
            _previousPosition = _rigidBody2D.position;
            _currentPosition = _previousPosition + _nextMovement;
            Velocity = (_currentPosition - _previousPosition) / Time.deltaTime;
            
            _rigidBody2D.MovePosition(_currentPosition);
            _nextMovement = Vector2.zero;
            
            CheckGrounded();
        }

        public void Move(Vector2 movement)
        {
            _nextMovement += movement;
        }

        private void CheckGrounded()
        {
            if (_collider2D.IsTouchingLayers(_groundMask))
            {
                IsGrounded = true;
            }
            else
            {
                IsGrounded = false;
            }
        }
    }
}