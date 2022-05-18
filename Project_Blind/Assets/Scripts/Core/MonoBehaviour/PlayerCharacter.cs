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
        private SpriteRenderer _renderer;
        
        [SerializeField] private float _jumpSpeed = 3f;
        [SerializeField] private float _jumpAbortSpeedReduction = 100f;
        [SerializeField] private float _gravity = 30f;
        
        [SerializeField] private float _maxSpeed = 5f;
        [SerializeField] private float groundAcceleration = 100f;
        [SerializeField] private float groundDeceleration = 100f;
        
        [SerializeField] private float _dashSpeed = 10f;
        [SerializeField] private float _defaultTime = 0.1f;
        private float _dashTime;
        private float _defaultSpeed;
        protected const float GroundedStickingVelocityMultiplier = 3f;    // This is to help the character stick to vertically moving platforms.
        private void Awake()
        {
            _moveVector = new Vector2();
            _characterController2D = GetComponent<CharacterController2D>();
            _animator = GetComponent<Animator>();
            _renderer = GetComponent<SpriteRenderer>();
            _defaultSpeed = _maxSpeed;
            _dashSpeed = 10f;
            _defaultTime = 0.1f;
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

        public void Dash()
        {
            if (_dashTime <= 0)
            {
                _maxSpeed = _defaultSpeed;
                if (InputController.Instance.Dash.Down)
                {
                    _dashTime = _defaultTime;
                }
            }
            else
            {
                _dashTime -= Time.deltaTime;
                _maxSpeed = _dashSpeed;
                int Playerflip;
                
                if (_renderer.flipX) Playerflip = -1;
                else Playerflip = 1;
                        
                float desiredSpeed = Playerflip * _maxSpeed * 0.1f;
                _moveVector.x = Mathf.MoveTowards(_moveVector.x, desiredSpeed, 0.5f);
            }   
        }

        /// <summary>
        /// 점프 키를 입력하면 위로 가속을 줍니다.
        /// </summary>
        public void Jump()
        {
            if (InputController.Instance.Jump.Down)
            {
                if(!(InputController.Instance.Vertical.Value < 0)) { // 아래 버튼을 누르지 않았다면
                    _moveVector.y = _jumpSpeed;
                }
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
                _moveVector.y = 0;
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

        public void setJumping(bool status = false) {
            this.gameObject.layer = status ? LayerMask.NameToLayer("UnitsJumping") : LayerMask.NameToLayer("Units");
        }

        public void UpdateVelocity()
        {
            Vector2 velocity = _characterController2D.Velocity;
            _animator.SetFloat("RunningSpeed",Mathf.Abs(velocity.x));
            _animator.SetFloat("VerticalSpeed",velocity.y);
        }

        public void UpdateFacing()
        {
            bool faceLeft = InputController.Instance.Horizontal.Value < 0f;
            bool faceRight = InputController.Instance.Horizontal.Value > 0f;
            if (faceLeft)
            {
                _renderer.flipX = true;
            }
            else if(faceRight)
            {
                _renderer.flipX = false;
            }
        } 
    }
}