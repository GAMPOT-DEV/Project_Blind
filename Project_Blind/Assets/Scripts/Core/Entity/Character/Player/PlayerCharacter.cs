using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Serialization;

namespace Blind
{
    /// <summary>
    /// 플레이어 캐릭터에 관한 클래스입니다.
    /// </summary>
    public class PlayerCharacter : Character,IGameManagerObj
    {
        public Vector2 _moveVector;
        private PlayerCharacterController2D _characterController2D;
        private ISkeletonComponent skeletonmecanim;
        public MeleeAttackable _attack;
        private Animator _animator;
        private SpriteRenderer _renderer;
        private Paringable _paring;
        public Vector2 _playerposition;
        public ScriptableObjects.PlayerCharacter Data;

        [SerializeField] private Transform _spawnPoint;
        public bool _isHurtCheck;
        public float _lastClickTime;
        public int _clickcount = 0;
        private float _dashTime;
        private float _defaultSpeed;
        private bool _candash = true;
        private int _dashCount;
        public bool isJump;
        protected const float GroundedStickingVelocityMultiplier = 3f;    // This is to help the character stick to vertically moving platforms.
        private GameObject _waveSense;
        public bool _isInvincibility;

        public int maxWaveGauge;
        [SerializeField] private int _currentWaveGauge = 30;
        public int CurrentWaveGauge
        {
            get { return _currentWaveGauge; }
            set
            {
                _currentWaveGauge = value;
                if (_currentWaveGauge < 0) _currentWaveGauge = 0;
                if (_currentWaveGauge > 30) _currentWaveGauge = 30;
                if (OnWaveGaugeChanged != null)
                    OnWaveGaugeChanged.Invoke(_currentWaveGauge);
            }
        }
        
        public int attackWaveGauge;
        public int paringWaveGauge;

        public bool isOnLava;

        public Action<int> OnWaveGaugeChanged;

        public void Awake()
        {
            base.Awake(Data);
            _moveVector = new Vector2();
            _characterController2D = GetComponent<PlayerCharacterController2D>();
            skeletonmecanim = GetComponent<SkeletonMecanim>();
            _attack = GetComponent<MeleeAttackable>();
            _paring = GetComponent<Paringable>();
            _animator = GetComponent<Animator>();
            _renderer = GetComponent<SpriteRenderer>();
            _defaultSpeed = Data.maxSpeed;
            //_dashSpeed = 10f;
            //_defaultTime = 0.2f;
            _dashCount = 1;
            

            ResourceManager.Instance.Destroy(ResourceManager.Instance.Instantiate("MapObjects/Wave/WaveSense").gameObject);
            _attack.Init(Data.attack_x,Data.attack_y,Data.damage);
            _paring.Init(Data.paring_x, Data.paring_y);


            // TEST
            if (FindObjectOfType<UI_FieldScene>() == null)
                UIManager.Instance.ShowSceneUI<UI_FieldScene>();
        }

        private void Start()
        {
            SceneLinkedSMB<PlayerCharacter>.Initialise(_animator, this);
        }

        public void OnFixedUpdate()
        {
            _characterController2D.Move(_moveVector);
            _characterController2D.OnFixedUpdate();
            _playerposition = new Vector2(transform.position.x, transform.position.y + 4f);
        }
        
        public void GroundedHorizontalMovement(bool useInput, float speedScale = 0.1f)
        {
            int flip = 0;
            bool isInputCheck;
            if (InputController.Instance.LeftMove.Held)
            {
                flip = -1;
            }
            else if (InputController.Instance.RightMove.Held)
            {
                flip = 1;
            }
            
            if (InputController.Instance.LeftMove.Held && InputController.Instance.RightMove.Held)
                flip = 0;

            if (InputController.Instance.LeftMove.Down || InputController.Instance.RightMove.Down) isInputCheck = false;
            else isInputCheck = true;
            float desiredSpeed = useInput ? flip * Data.maxSpeed * speedScale : 0f;
            float acceleration = useInput && isInputCheck ? Data.groundAcceleration : Data.groundDeceleration;
            _moveVector.x = Mathf.MoveTowards(_moveVector.x, desiredSpeed, acceleration * Time.deltaTime);
        }

        public void DashStart()
        {
            StartCoroutine(Dash());
        }
        public IEnumerator Dash()
        {
            _animator.SetTrigger("Dash");
            _candash = false;
            float originalGravity = Data.gravity;
            float desiredSpeed = GetFacing() * Data.dashSpeed * 0.1f;
            Debug.Log(desiredSpeed);
            if (_characterController2D.IsGrounded && _moveVector.x == 0)
            {
                _moveVector.x = desiredSpeed * 2;
                _moveVector.y = 0;
            }
            else
            {
                _moveVector.x = desiredSpeed;
                _moveVector.y = 0;
            }

            yield return new WaitForSeconds(Data.defaultTime);
            Data.gravity = originalGravity;
            yield return new WaitForSeconds(1f);
            _candash = true;

        }
        public bool CheckForDash()
        {
            return InputController.Instance.Dash.Down && !InputController.Instance.DownJump.Held && !isOnLava && _candash;
        }

        /// <summary>
        /// 점프 키를 입력하면 위로 가속을 줍니다.
        /// </summary>
        public void Jump()
        {
            if (InputController.Instance.Jump.Down)
            {
                if(!(InputController.Instance.DownJump.Held)) { // 아래 버튼을 누르지 않았다면
                    _moveVector.y = Data.jumpSpeed;
                }
                _animator.SetTrigger("Jump");
            }
        }

        public void WaveSensePress()
        {
            if (InputController.Instance.Wave.Down && _currentWaveGauge >= 10)
            {
                if (WaveSense.IsUsing)
                    return;

                CurrentWaveGauge -= 10;
                SoundManager.Instance.Play("WaveSound", Define.Sound.Effect);

                var waveSense = ResourceManager.Instance.Instantiate("WaveSense").GetComponent<WaveSense>();
                waveSense.transform.position = transform.position;
                waveSense.StartSpread();
            }
        }

        public void StopMoveY()
        {
            _moveVector.y = 0;
        }
        
        
        public void UpdateJump()
        {
            if (!InputController.Instance.Jump.Held && _moveVector.y > 0.0f)
            {
                _moveVector.y -= Data.jumpAbortSpeedReduction * Time.deltaTime;
            }
        }
        /// <summary>
        /// 중력을 적용합니다.
        /// </summary>
        public void AirborneVerticalMovement(float _gravity)
        {
            if (Mathf.Approximately(_moveVector.y, 0f) )//|| CharacterController2D.IsCeilinged && _moveVector.y > 0f) 나중에 천장 코드 구현되면 그 때 수정
            {
                _moveVector.y = 0;
            }
            _moveVector.y -= _gravity * Time.deltaTime;
        }
        public void AirborneHorizontalMovement()
        {
            float desiredSpeed = InputController.Instance.Horizontal.Value * Data.maxSpeed;

            float acceleration;

            if (InputController.Instance.Horizontal.ReceivingInput)
                acceleration = Data.airborneAccelProportion;
            else
                acceleration = Data.airborneDecelProportion;

            _moveVector.x = Mathf.MoveTowards(_moveVector.x, desiredSpeed, acceleration * Time.deltaTime);
        }
        public void GroundedVerticalMovement()
        {
            _moveVector.y -= Data.gravity * Time.deltaTime;

            if (_moveVector.y < - Data.gravity * Time.deltaTime * GroundedStickingVelocityMultiplier)
            {
                _moveVector.y = - Data.gravity * Time.deltaTime * GroundedStickingVelocityMultiplier;
            }
        }

        public void CheckForGrounded()
        {
            bool grounded = _characterController2D.IsGrounded;
            isJump = grounded;
            _animator.SetBool("Grounded",grounded);
        }

        public bool CheckForParing()
        {
            return InputController.Instance.Paring.Down;
        }

        public void Paring()
        {
            _animator.SetTrigger("Paring");
        }

        public void EnableParing()
        {
            _paring.EnParing();
        }

        public void DisableParing()
        {
            _paring.DisableParing();
        }


        public bool CheckForAttack()
        {
            return InputController.Instance.Attack.Down;
        }

        public bool CheckForAttackTime()
        {
            return Time.time - _lastClickTime > Data.maxComboDelay;
        }

        public void ReAttackSize(int x, int y)
        {
            _attack.Init(x, y);
        }
        public void MeleeAttack()
        {
            _animator.SetBool("Attack", true);
        }

        public void MeleeAttackCombo1()
        {
            _animator.SetBool("Attack2", true);
        }

        public void MeleeAttackCombo2()
        {
            _animator.SetBool("Attack3", true);
        }

        public void MeleeAttackCombo3()
        {
            _animator.SetBool("Attack4", true);
        }

        public void MeleeAttackComoEnd()
        {
            _animator.SetBool("Attack", false);
            _animator.SetBool("Attack2" ,false);
            _animator.SetBool("Attack3", false);
            _animator.SetBool("Attack4", false);
            _clickcount = 0;
        }

        public bool CheckForPowerAttack()
        {
            return InputController.Instance.Attack.Held;
        }

        public bool CheckForUpKey()
        {
            return InputController.Instance.Attack.Up;
        }
        public void AttackableMove(float newMoveVector)
        {
            _moveVector.x = newMoveVector;
        }

        public void enableAttack()
        {
            _attack.EnableDamage();
        }

        public void DisableAttack()
        {
            _attack.DisableDamage();
        }
        /// <summary>
        /// 아래 키를 누른 상태에 점프키를 눌렀는지 체크
        /// </summary>
        public bool CheckForFallInput()
        {
            return InputController.Instance.DownJump.Held&& InputController.Instance.Dash.Down;
        }

        /// <summary>
        /// 레이캐스트에 맞은 오브젝트가 PlatformEffector를 가지고있는지 판별 후 있다면 아래점프 실행
        /// </summary>
        public void MakePlatformFallthrough()
        {
            _characterController2D.MakePlatformFallthrough();
        }

        protected override void onHurt()
        {
            _animator.SetTrigger("Hurt");
            _isHurtCheck = true;
        }

        protected override void HurtMove(Facing enemyFacing)
        {
            _moveVector.x = Data.hurtMove * (float)enemyFacing;
        }

        public void Deed()
        {
            _animator.SetBool("Dead", true);
            StartCoroutine(DieRespawn());
        }
        IEnumerator DieRespawn()
        {
            InputController.Instance.ReleaseControl(true);
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(UI_ScreenFader.FadeScenOut());
            
            Respawn();
            yield return new WaitForEndOfFrame();
            yield return StartCoroutine(UI_ScreenFader.FadeSceneIn());
            InputController.Instance.GainControl();
        }

        public void DieStopVector(Vector2 stop)
        {
            _moveVector = stop;
        }

        public void Respawn()
        {
            RespawnFacing();
            Hp.ResetHp();
            _animator.SetTrigger("Respawn");
            _animator.SetBool("Dead", false);
            gameObject.transform.position = _spawnPoint.position;
        }

        public void GetItem()
        {
            _animator.SetTrigger("Item");
        }

        public bool CheckForItemT()
        {
            return InputController.Instance.ItemT.Down;
        }

        public void ItemT()
        {
            _animator.SetTrigger("ItemT");
        }

        public void ThrowItem()
        {
            var bullet = ResourceManager.Instance.Instantiate("Item/WaveBullet").GetComponent<WaveBullet>();
            bullet.transform.position = _playerposition;
            if(_renderer == null) bullet.GetFacing(skeletonmecanim.Skeleton.FlipX);
            else bullet.GetFacing(_renderer.flipX);
        }
        public void Talk()
        {
            _animator.SetBool("Talk", true);
        }

        public void UnTalk()
        {
            _animator.SetBool("Talk" , false);
        }
        
        public void UpdateVelocity()
        {
            Vector2 velocity = _characterController2D.Velocity;
            _animator.SetFloat("RunningSpeed", Math.Abs(velocity.x));
            _animator.SetFloat("VerticalSpeed",velocity.y);
        }
        
        public void UpdateFacing()
        {
            bool faceLeft = InputController.Instance.LeftMove.Held;
            bool faceRight = InputController.Instance.RightMove.Held;
            if (faceLeft)
            {
                if(faceRight) return;
                if(_renderer == null) skeletonmecanim.Skeleton.FlipX = false;
                else _renderer.flipX = false;
            }
            else if(faceRight)
            {
                if(_renderer == null) skeletonmecanim.Skeleton.FlipX = true;
                else _renderer.flipX = true;
            }
        }
        

        public bool CheckForSkill()
        {
            return InputController.Instance.Skill.Down;
        }

        public void Skill()
        {
            _animator.SetTrigger("Skill");
        }
        
        public void RespawnFacing()
        {
            if (_renderer == null) skeletonmecanim.Skeleton.FlipX = true;
            else _renderer.flipX = true;
        }

        public int GetFacing()
        {
            if (_renderer == null) return skeletonmecanim.Skeleton.FlipX ? 1 : -1;
            else return _renderer.flipX ? 1 : -1;
        }

        public void Log() {
            Debug.Log(_characterController2D.IsGrounded ? "땅" : "공중");
        }
        public void DebuffOn()
        {
            Debug.Log("디버프 걸림");
            isOnLava = true;
            _defaultSpeed -= 2.0f;
            Data.jumpSpeed = 0.3f;
            StartCoroutine(GetDotDamage());

        }

        IEnumerator GetDotDamage()
        {
            while (isOnLava)
            {
                // hp를 깎음
                Hp.GetDamage(1f);
                yield return new WaitForSeconds(0.5f);
            }
            DebuffOff();
        }

        public void DebuffOff()
        {
            _defaultSpeed += 2.0f;
            Data.jumpSpeed = 0.7f;
            Debug.Log("디버프 풀림");

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Floor"))
            {
                isOnLava = false;
            }
            //if (collision.transform.parent !=null)
            //{
            //    if(collision.transform.parent.gameObject.name == "Floors")
            //    {
            //        isOnLava = false;
            //    }
            //}
        }

        public void  Key()
        {
            
            
        }
    }
}