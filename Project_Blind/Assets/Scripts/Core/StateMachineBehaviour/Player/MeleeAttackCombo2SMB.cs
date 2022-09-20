using UnityEngine;
using UnityEngine.Animations;

namespace Blind
{
    public class MeleeAttackCombo2SMB: SceneLinkedSMB<PlayerCharacter>
    {
        UI_FieldScene ui = null;
        private bool _powerAttack = false;
        private bool _isOnClick = false;
        private bool _checkForPowerAttack = false;

        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _isOnClick = false;
            _monoBehaviour.ReAttackSize(4,2);
            _monoBehaviour.StopMoveY();
            SoundManager.Instance.Play("주인공 공격 사운드", Define.Sound.Effect);
        }
        public override void OnSLStatePostEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex)
        {
            if (_monoBehaviour.CheckForPowerAttack() && _monoBehaviour.CurrentWaveGauge > 10) {
                animator.speed = 0.1f;
                _checkForPowerAttack = true;
                _monoBehaviour.EndPowerAttack();
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
                if(_monoBehaviour.isJump) _monoBehaviour.AttackableMove(_monoBehaviour.Data.attackMove * (float)_monoBehaviour.GetFacing());
                _monoBehaviour.PlayAttackFx(1,_monoBehaviour.GetFacing());
                _monoBehaviour.enableAttack();
            }
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {

            if (!_monoBehaviour.isJump)
            {
                _monoBehaviour.AirborneVerticalMovement(1f);
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
            if (_monoBehaviour._clickcount >= 3) 
                _monoBehaviour.MeleeAttackCombo2();
            if (_monoBehaviour._clickcount == 0) 
                _monoBehaviour.MeleeAttackComoEnd();

            if ((_monoBehaviour.CheckForUpKey() && _checkForPowerAttack && !_powerAttack)
                || (_monoBehaviour.isPowerAttackEnd &&!_powerAttack))
            {
                animator.speed = 1.0f;
                _monoBehaviour._attack.DamageReset(_monoBehaviour.Data.powerAttackdamage);
                _monoBehaviour.enableAttack();
                _monoBehaviour.AttackableMove(_monoBehaviour.Data.attackMove * (float)_monoBehaviour.GetFacing());
                _monoBehaviour.CurrentWaveGauge -= 10;
                _monoBehaviour.isPowerAttackEnd = false;

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
            }
        }
        public override void OnSLStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(_monoBehaviour._clickcount == 2)
                _monoBehaviour.MeleeAttackComoEnd();
            _monoBehaviour.DisableAttack();
            _powerAttack = false;
            SoundManager.Instance.StopEffect();
        }
    }
}