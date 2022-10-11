using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Serialization;
using Cinemachine;

namespace Blind
{
    /// <summary>
    /// 플레이어 캐릭터에 관한 클래스입니다.
    /// </summary>
    public class PlayerCharacter : Character,IGameManagerObj
    {
        public Vector2 _moveVector;
        public PlayerCharacterController2D _characterController2D;
        private ISkeletonComponent skeletonmecanim;
        public MeleeAttackable _attack;
        private Animator _animator;
        [SerializeField] public Paringable _paring;
        public Vector2 _playerposition;
        public ScriptableObjects.PlayerCharacter Data;
        private Rigidbody2D rigid;
        public CinemachineImpulseSource _source;
        public CinemachineVirtualCamera _camera;

        [FormerlySerializedAs("_spawnPoint")] public Transform spawnPoint;
        [SerializeField] private Transform _bulletPoint;
        public bool _isHurtCheck;
        public float _lastClickTime;
        public int _clickcount = 0;
        private float _dashTime;
        private float _defaultSpeed;
        private bool _candash = true;
        public bool isCheck = false;
        public float nextDash_x;
        public bool isJump;
        protected const float GroundedStickingVelocityMultiplier = 3f;    // This is to help the character stick to vertically moving platforms.
        private GameObject _waveSense;
        public bool _isInvincibility;
        public bool isPowerAttackEnd;
        public bool isPowerAttack;
        public bool isParingCheck = false;
        public bool isInputCheck;
        //public int maxWaveGauge;
        private bool _isInput = false;
        public bool TalismanMoney = false;
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
        private float desiredSpeed;
        private float currentmovevector_x;
        public float gravity;
        private bool soundPlay;
        public bool bulletCheck;
        public Action<int> OnWaveGaugeChanged;

        public void Awake()
        {
            base.Awake(Data);
            _moveVector = new Vector2();
            _characterController2D = GetComponent<PlayerCharacterController2D>();
            skeletonmecanim = GetComponent<SkeletonMecanim>();
            _attack = GetComponent<MeleeAttackable>();
            _animator = GetComponent<Animator>();
            _renderer = GetComponent<SpriteRenderer>();
            rigid = GetComponent<Rigidbody2D>();
            _source = GetComponent<CinemachineImpulseSource>();
            _camera = GetComponent<CinemachineVirtualCamera>();
            _camera = GameObject.Find("CM Virtual Camera").GetComponent<CinemachineVirtualCamera>();
            _defaultSpeed = Data.maxSpeed;
            //_dashSpeed = 10f;
            //_defaultTime = 0.2f;
            gravity = Data.gravity;
            

            ResourceManager.Instance.Destroy(ResourceManager.Instance.Instantiate("MapObjects/Wave/WaveSense").gameObject);
            _attack.Init(Data.attack_x,Data.attack_y,Data.damage);


            // TEST
            if (FindObjectOfType<UI_FieldScene>() == null)
                UIManager.Instance.ShowSceneUI<UI_FieldScene>();
            OnWaveGaugeChanged.Invoke(_currentWaveGauge);
        }

        private void Start()
        {
            SceneLinkedSMB<PlayerCharacter>.Initialise(_animator, this);
            Hp.SetHealth();
            DataManager.Instance.ClearBagData();
        }

        public void OnFixedUpdate()
        {
            _characterController2D.Move(_moveVector);
            _characterController2D.OnFixedUpdate();
            _playerposition = new Vector2(transform.position.x, transform.position.y + 2.5f);
        }

        public void SetPlayerValue(PlayerCharacterData playerCharacterData)
        {
            if (playerCharacterData == null) return;
            Hp.SetHealth(playerCharacterData.Hp);
            CurrentWaveGauge = playerCharacterData.CurrentWaveGage;
            transform.position = SceneController.SetDestination(SceneController.Instance.DestinationTag);
        }
        
        public void GroundedHorizontalMovement(bool useInput, float speedScale = 0.1f, bool isJumpAttack = false)
        {
            int flip = 0;
            float speed = 0;
            float jumpattackspeed = 3f;
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


            if (InputController.Instance.LeftMove.Down || InputController.Instance.RightMove.Down)
            {
                isInputCheck = false;
                _animator.SetBool("RunEnd", false);
                SoundManager.Instance.StopEffect();
            }
            else
            {
                isInputCheck = true;
            }

            if (!InputController.Instance.LeftMove.Held && !InputController.Instance.RightMove.Held)
            {
                _animator.SetBool("RunEnd", true);
            }

            speed = !isJumpAttack ? Data.maxSpeed : jumpattackspeed; //일단 여기
            float desiredSpeed = useInput ? flip * speed * speedScale : 0f;
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
            isCheck = false;
            float originalGravity = Data.gravity;
            isCheck = true;
            if(_moveVector.x == 0)
                desiredSpeed = (float)GetFacing() * Data.dashSpeed * 0.05f;
            else
                desiredSpeed = (float)GetFacing() * (Data.dashSpeed+ 3) * 0.05f;
            _moveVector.y = 0;

            currentmovevector_x = _moveVector.x;
            
            yield return new WaitForSeconds(Data.defaultTime);
            Data.gravity = originalGravity;
            yield return new WaitForSeconds(1f);
            _candash = true;
            isCheck = false;
        }

        public void StopDash()
        {
            _moveVector.x = Mathf.MoveTowards(currentmovevector_x, desiredSpeed, 500 * Time.deltaTime);
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
                var obj = ResourceManager.Instance.Instantiate("FX/EnvFx/JumpFx");
                obj.transform.position = transform.position + Vector3.up * 2;
                SoundManager.Instance.Play("Jump",Define.Sound.Effect);
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
            _characterController2D.isDown = true;
            if (Mathf.Approximately(_moveVector.y, 0f) )//|| CharacterController2D.IsCeilinged && _moveVector.y > 0f) 나중에 천장 코드 구현되면 그 때 수정
            {
                _moveVector.y = 0;
            }

            _moveVector.y -= _gravity * Time.deltaTime;
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
        

        public void ReAttackSize(int x, int y, int damege)
        {
            _attack.Init(x, y, damege);
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
            return InputController.Instance.PowerAttack.Down;
        }

        public void EndPowerAttack()
        {
            StartCoroutine(isEndPowerAttack());
        }

        IEnumerator isEndPowerAttack()
        {
            yield return new WaitForSeconds(0.6f);
            isPowerAttackEnd = true;
        }

        public bool CheckForUpKey()
        {
            return InputController.Instance.PowerAttack.Up;
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

        public override void HitSuccess()
        {
            CurrentWaveGauge += attackWaveGauge;
        }

        protected override void onHurt()
        {
            _animator.SetTrigger("Hurt");
            _isHurtCheck = true;
            _moveVector.y = 0;
        }

        protected override void HurtMove(Facing enemyFacing)
        {
            _moveVector.x = Data.hurtMove * (float)enemyFacing;
        }

        public void OnCaught()
        {
            _animator.SetTrigger("Caught");
        }

        public void Deed()
        {
            _animator.SetBool("Dead", true);
            StartCoroutine(DieRespawn());
        }
        IEnumerator DieRespawn()
        {
            InputController.Instance.ReleaseControl(true);
            DataManager.Instance.SubMoney(DataManager.Instance.GetMoney() / 2);
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(UI_ScreenFader.FadeScenOut());
            
            Respawn();
            GameManager.Instance.ResetStage();
            yield return new WaitForEndOfFrame();
            yield return StartCoroutine(UI_ScreenFader.FadeSceneIn());
            InputController.Instance.GainControl();
            var gameObject = ResourceManager.Instance.Instantiate("FX/EnvFx/Respawn");
            gameObject.transform.position = GameManager.Instance.Player.transform.position;
        }

        public void DieStopVector(Vector2 stop)
        {
            _moveVector = stop;
        }

        public void Respawn()
        {
            RespawnFacing();
            Hp.ResetHp();
            DebuffOff();
            _animator.SetTrigger("Respawn");
            _animator.SetBool("Dead", false);
            gameObject.transform.position = GameManager.Instance.GetTransform().position;
        }

        public void GetItem()
        {
            _animator.SetTrigger("Item");
        }

        public bool CheckForItemT()
        {
            return InputController.Instance.ItemT.Down && DataManager.Instance.HaveBagItem(Define.BagItem.WaveStick) && !bulletCheck;
        }

        public bool CheckForItemUsing()
        {
            return InputController.Instance.ItemUsing.Down && DataManager.Instance.HaveBagItem(Define.BagItem.Potion);
        }

        public void ItemUsing()
        {
            _animator.SetTrigger("Item");
        }
        public void ItemT()
        {
            _animator.SetTrigger("ItemT");
        }

        public void ThrowItem()
        {
            var bullet = ResourceManager.Instance.Instantiate("Item/WaveBullet").GetComponent<WaveBullet>();
            bullet.Init(this);
            bulletCheck = true;
            bullet.transform.position = _bulletPoint.position;
            if(_renderer == null) bullet.GetFacing(GetFacing());
            else bullet.GetFacing(GetFacing());
        }

        public void HpHeal()
        {
            Hp.GetHeal(3);
        }
        public void Talk()
        {
            InputController.Instance.ReleaseControl();
            _moveVector = Vector2.zero;
            _animator.SetBool("Talk", true);
        }

        public void UnTalk()
        {
            InputController.Instance.GainControl();
            _animator.SetBool("Talk" , false);
        }
        
        public void UpdateVelocity()
        {
            Vector2 velocity = _characterController2D.Velocity;
            _animator.SetFloat("RunningSpeed", Mathf.Abs(velocity.x));
            _animator.SetFloat("VerticalSpeed",Mathf.Abs(velocity.y));
        }
        
        public void UpdateFacing()
        {
            bool faceLeft = InputController.Instance.LeftMove.Held;
            bool faceRight = InputController.Instance.RightMove.Held;
            if (faceLeft)
            {
                if(faceRight) return;
                if(_renderer == null) skeletonmecanim.Skeleton.ScaleX = 1;
            }
            else if(faceRight)
            {
                if(_renderer == null) skeletonmecanim.Skeleton.ScaleX = -1;
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
            if (_renderer == null) skeletonmecanim.Skeleton.ScaleX = 1;
        }

        public override Facing GetFacing()
        {
            if (_renderer == null)
            {
                return skeletonmecanim.Skeleton.ScaleX < 0 ? Facing.Right : Facing.Left;
            }

            return Facing.Right;
        }

        public void Log() {
            //Debug.Log(_characterController2D.IsGrounded ? "땅" : "공중");
        }
        public void DebuffOn()
        {
            isOnLava = true;
            _defaultSpeed -= 1.0f;
            Data.jumpSpeed = 0.5f;
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
            isOnLava = false;
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

        public void ChangeHp(int value)
        {
            Hp.ChangeHp(value);
        }

        public void ChangeDamage(int value)
        {
            Data.damage += value;
            _attack.DamageReset(Data.damage);
        }

        public void ChangeWaveGauge(int value)
        {
            Data.maxWaveGauge += value;
            if (_currentWaveGauge > Data.maxWaveGauge)
                _currentWaveGauge = Data.maxWaveGauge;
        }

        public void ChangeMoneyProb(bool value)
        {
            TalismanMoney = value;
        }

        public void ChangeSpeed(int value)
        {
            Data.maxSpeed += value;
            Data.dashSpeed += value;
        }

        public override void PlayAttackFx(int level)
        {
            base.PlayAttackFx(level);
            if (isPowerAttack)
            {
                level += 4;
            }
            transform.GetChild(1).GetChild(level).GetComponent<AttackFX>().Play(GetFacing());
        }
    }
}