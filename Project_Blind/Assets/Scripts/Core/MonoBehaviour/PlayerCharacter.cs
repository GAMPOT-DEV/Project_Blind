using System;
using System.Net.NetworkInformation;
using UnityEngine;

namespace Blind
{
    /// <summary>
    /// 플레이어 캐릭터에 관한 클래스입니다.
    /// </summary>
    public class PlayerCharacter : MonoBehaviour,IGameManagerObj
    {
        private Vector2 _moveVector;
        private CharacterController2D _characterController2D;
        private Animator _animator;
        
        [SerializeField] private float _jumpSpeed = 3f;
        [SerializeField] private float _jumpAbortSpeedReduction = 100f;
        [SerializeField] private float _gravity = 30f;
        
        [SerializeField] private float _maxSpeed = 5f;
        [SerializeField] private float groundAcceleration = 100f;
        [SerializeField] private float groundDeceleration = 100f;
        protected const float GroundedStickingVelocityMultiplier = 3f;    // This is to help the character stick to vertically moving platforms.
        private void Awake()
        {
            _moveVector = new Vector2();
            _characterController2D = GetComponent<CharacterController2D>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            SceneLinkedSMB<PlayerCharacter>.Initialise(_animator, this);
        }

        public void OnFixedUpdate()
        {
            _characterController2D.Move(_moveVector);
            _characterController2D.OnFixedUpdate();
        }
        
        public void GroundedHorizontalMovement(bool useInput, float speedScale = 0.1f)
        {
            float desiredSpeed = useInput ? InputController.Instance.Horizontal.Value * _maxSpeed * speedScale : 0f;
            float acceleration = useInput && InputController.Instance.Horizontal.ReceivingInput ? groundAcceleration : groundDeceleration;
            _moveVector.x = Mathf.MoveTowards(_moveVector.x, desiredSpeed, acceleration * Time.deltaTime);
        }

        /// <summary>
        /// 점프 키를 입력하면 위로 가속을 줍니다.
        /// </summary>
        public void Jump()
        {
            if (InputController.Instance.Jump.Down)
            {
                _moveVector.y = _jumpSpeed;
                _animator.SetTrigger("Jump");
            }
        }
        
        public void UpdateJump()
        {
            if (!InputController.Instance.Jump.Held && _moveVector.y > 0.0f)
            {
                _moveVector.y -= _jumpAbortSpeedReduction * Time.deltaTime;
            }
        }
        /// <summary>
        /// 중력을 적용합니다.
        /// </summary>
        public void AirborneVerticalMovement()
        {
            if (Mathf.Approximately(_moveVector.y, 0f) )//|| CharacterController2D.IsCeilinged && _moveVector.y > 0f) 나중에 천장 코드 구현되면 그 때 수정
            {
                _moveVector.y = 0f;
            }
            _moveVector.y -= _gravity * Time.deltaTime;
        }
        public void GroundedVerticalMovement()
        {
            _moveVector.y -= _gravity * Time.deltaTime;

            if (_moveVector.y < -_gravity * Time.deltaTime * GroundedStickingVelocityMultiplier)
            {
                _moveVector.y = -_gravity * Time.deltaTime * GroundedStickingVelocityMultiplier;
            }
        }

        public void CheckForGrounded()
        {
            bool grounded = _characterController2D.IsGrounded;
            
            _animator.SetBool("Grounded",grounded);
        }

        public void UpdateVelocity()
        {
            Vector2 velocity = _characterController2D.Velocity;
            _animator.SetFloat("RunningSpeed",Mathf.Abs(velocity.x));
            _animator.SetFloat("VerticalSpeed",velocity.y);
        }
    }
}