using System;
using System.Collections;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Blind
{
    /// <summary>
    /// 플레이어 캐릭터에 관한 클래스입니다.
    /// </summary>
    public class PlayerCharacter : MonoBehaviour,IGameManagerObj
    {
        private Vector2 _moveVector;
        private PlayerCharacterController2D _characterController2D;
        private UnitHP _damage;
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
        private int _dashCount;
        protected const float GroundedStickingVelocityMultiplier = 3f;    // This is to help the character stick to vertically moving platforms.
        private GameObject _waveSense;
        private GameObject _enemyObject;
        private void Awake()
        {
            _moveVector = new Vector2();
            _characterController2D = GetComponent<PlayerCharacterController2D>();
            _damage = new UnitHP(10);
            _animator = GetComponent<Animator>();
            _renderer = GetComponent<SpriteRenderer>();
            _defaultSpeed = _maxSpeed;
            _dashSpeed = 10f;
            _defaultTime = 0.1f;
            _dashCount = 1;
            
			ResourceManager.Instance.Destroy(ResourceManager.Instance.Instantiate("WaveSense").gameObject);
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
                if (_dashCount == 1)
                {
                    if (InputController.Instance.Jump.Down && InputController.Instance.Vertical.Value>-float.Epsilon)
                    {
                        _dashCount--;
                        _dashTime = _defaultTime;
                        StartCoroutine(ReturnDashCount());
                    }
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

        IEnumerator ReturnDashCount()
        {
            yield return new WaitForSeconds(1f);
            _dashCount = 1;
        }

        /// <summary>
        /// 점프 키를 입력하면 위로 가속을 줍니다.
        /// </summary>
        public void Jump()
        {
            if (InputController.Instance.Vertical.Value >0)
            {
                if(!(InputController.Instance.Vertical.Value < 0)) { // 아래 버튼을 누르지 않았다면
                    _moveVector.y = _jumpSpeed;
                }
                _animator.SetTrigger("Jump");
            }
        }

        public void WaveSensePress()
        {
            if (InputController.Instance.Wave.Down)
            {
                var waveSense = ResourceManager.Instance.Instantiate("WaveSense").GetComponent<WaveSense>();
                waveSense.transform.position = transform.position;
			    waveSense.StartSpread();
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
        public bool CheckEnemy()
        {
            bool isEnemy = false;
            Collider2D[] _col;
            
            if (_renderer.flipX)
                _col = Physics2D.OverlapBoxAll(
                    new Vector2(gameObject.transform.position.x - 1, gameObject.transform.position.y),
                    new Vector2(1f, 3), 0);
            else
                _col = Physics2D.OverlapBoxAll(
                    new Vector2(gameObject.transform.position.x + 1, gameObject.transform.position.y),
                    new Vector2(1f, 3), 0);
            
            foreach (Collider2D colliders in _col)
            {
                Debug.Log(colliders.tag);
                if (colliders.tag .Equals("Enemy"))
                {
                    _enemyObject = colliders.gameObject;
                    isEnemy = true;
                }
            }

            return isEnemy;
        }   

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y , gameObject.transform.position.z), new Vector3(1,3,0));
        }

        public bool CheckForParing()
        {
            return InputController.Instance.Paring.Down;
        }

        public void Paring()
        {
            _animator.SetTrigger("Paring");
        }
        
        public void ParingObjCheck()
        {
            
        }
        public void EnemyStateCheck()
        {
            if (_enemyObject.GetComponent<EnemyCharacter>().isAttack)
            {
                StartCoroutine(Invincibility());
            }
        }

        IEnumerator Invincibility()
        {
            _damage.Invincibility();
            yield return new WaitForSeconds(0.5f);
            _damage.unInvicibility();
            // 나중에 데미지관련 class만들어서 무적 넣을 예정
        }
        /// <summary>
        /// 아래 키를 누른 상태에 점프키를 눌렀는지 체크
        /// </summary>
        public bool CheckForFallInput()
        {
            return InputController.Instance.Vertical.Value < -float.Epsilon && InputController.Instance.Jump.Down;
        }

        /// <summary>
        /// 레이캐스트에 맞은 오브젝트가 PlatformEffector를 가지고있는지 판별 후 있다면 아래점프 실행
        /// </summary>
        public void MakePlatformFallthrough()
        {
            _characterController2D.MakePlatformFallthrough();
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
        public void Log() {
            Debug.Log(_characterController2D.IsGrounded ? "땅" : "공중");
        }
    }
}