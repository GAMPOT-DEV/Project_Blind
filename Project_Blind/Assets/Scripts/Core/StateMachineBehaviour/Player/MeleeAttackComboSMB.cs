using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Animations;
namespace Blind
{
    public class MeleeAttackComboSMB: SceneLinkedSMB<PlayerCharacter>
    {
        UI_FieldScene ui = null;
        private bool _powerAttack = false;
        private bool _isOnClick = false;
        private bool _checkForPowerAttack = false;
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _isOnClick = false;
            _monoBehaviour.ReAttackSize(6, 5, _monoBehaviour.Data.damage);
            if(!_monoBehaviour.isPowerAttack) SoundManager.Instance.Play("Player/Attack/1타", Define.Sound.Effect);
            

        }

        public override void OnSLStatePostEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex) {
            if (_monoBehaviour.isPowerAttack && _monoBehaviour.CurrentWaveGauge >= 10)
            {
                animator.speed = 0.2f;
                _checkForPowerAttack = true;
                _monoBehaviour.EndPowerAttack();
                _monoBehaviour.transform.GetChild(1).GetChild(8).GetComponent<AttackFX>().Play(_monoBehaviour.GetFacing());
                if (ui == null)
                {
                    ui = FindObjectOfType<UI_FieldScene>();
                }
                if (ui != null)
                {
                    ui.StartCharge();
                }
            }
            else
            {
                if (_monoBehaviour.isJump)
                {
                    _monoBehaviour.AttackableMove(_monoBehaviour.Data.attackMove * (float)_monoBehaviour.GetFacing());
                }
                _monoBehaviour.PlayAttackFx(0,_monoBehaviour.GetFacing());
                _monoBehaviour.enableAttack();
            }
            
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            _monoBehaviour.UpdateVelocity();
            if (!_monoBehaviour.isJump)
            {
                Debug.Log("실행됨!!");
                _monoBehaviour.AirborneVerticalMovement(_monoBehaviour.gravity + 4);
                _monoBehaviour.UpdateJump();
                _monoBehaviour.CheckForGrounded();
                _monoBehaviour.GroundedHorizontalMovement(true);
                _monoBehaviour.UpdateFacing();
            }
            else _monoBehaviour.GroundedHorizontalMovement(false);
            
            if (_monoBehaviour.CheckForAttack() && !_isOnClick)
            {
                _monoBehaviour._clickcount++;
                _monoBehaviour._clickcount = Mathf.Clamp(_monoBehaviour._clickcount, 0, 4);
                _isOnClick = true;
            }

            if (_monoBehaviour._clickcount >= 2 && _monoBehaviour.isJump)
            {
                _monoBehaviour.MeleeAttackCombo1();
                Debug.Log("dd");
            }

            if (_monoBehaviour.CheckForPowerAttack() && _monoBehaviour.CurrentWaveGauge >= 10)
            {
                _monoBehaviour.MeleeAttackCombo1();
                _monoBehaviour.isPowerAttack = true;
            }
            if(_monoBehaviour.CheckForDeed())
            {
                _monoBehaviour.Deed();
            }
            
            if (_monoBehaviour.CheckForDash())
            {
                _monoBehaviour.DashStart();
            }
            
            if(_monoBehaviour.CheckForParing())
                _monoBehaviour.Paring();

            if ((_monoBehaviour.isPowerAttackEnd &&!_powerAttack)){
                Debug.Log("강공격!");
                animator.speed = 1.0f;
                _monoBehaviour._attack.DamageReset(_monoBehaviour.Data.powerAttackdamage);
                _monoBehaviour.enableAttack();
                _monoBehaviour.CurrentWaveGauge -= 10;
                _monoBehaviour.isPowerAttackEnd = false;
                _monoBehaviour.PlayAttackFx(4,_monoBehaviour.GetFacing());

                if (ui == null)
                {
                    ui = FindObjectOfType<UI_FieldScene>();
                }
                if (ui != null)
                {
                    ui.StopCharge();
                }
                _powerAttack = true;
                _checkForPowerAttack = false;
                _monoBehaviour.isPowerAttack = false;
            }

        }
        public override void OnSLStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            
            if (_monoBehaviour._clickcount == 1)
            {
                _monoBehaviour.MeleeAttackComoEnd();
            }
            //Debug.Log(_monoBehaviour._clickcount);
            _monoBehaviour._attack.DefultDamage();
            _monoBehaviour.DisableAttack();
            _powerAttack = false;
        }
    }
}