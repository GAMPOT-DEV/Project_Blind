using UnityEngine;
using UnityEngine.Animations;
namespace Blind
{
    public class MeleeAttackComboSMB: SceneLinkedSMB<PlayerCharacter>
    {
        private bool isPowerAttack;
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _monoBehaviour.StopMoveY();
        }

        public override void OnSLStatePostEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex) {
            if (_monoBehaviour.CheckForPowerAttack() && _monoBehaviour.CurrentWaveGauge >= 10)
            {
                animator.speed = 0.06f;
                isPowerAttack = true;
            }
            else
            {
                if (_monoBehaviour.isJump)
                {
                    _monoBehaviour.AttackableMove(_monoBehaviour._attackMove * _monoBehaviour.GetFacing());
                }
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
            
            if (_monoBehaviour.CheckForAttack())
            {
                _monoBehaviour._lastClickTime = Time.time;
                _monoBehaviour._clickcount++;
                _monoBehaviour._clickcount = Mathf.Clamp(_monoBehaviour._clickcount, 0, 4);
            }
            if (_monoBehaviour.CheckForAttackTime())
                _monoBehaviour._clickcount = 0; 
            if (_monoBehaviour._clickcount == 0)
                _monoBehaviour.MeleeAttackComoEnd();
            if (_monoBehaviour._clickcount >= 2)
                _monoBehaviour.MeleeAttackCombo1();

            if (isPowerAttack)
            {
                if (_monoBehaviour.CheckForUpKey() && _monoBehaviour.CurrentWaveGauge >= 10)
                {
                    animator.speed = 1.0f;
                    _monoBehaviour._attack.DamageReset(_monoBehaviour._powerAttackdamage);
                    _monoBehaviour.enableAttack();
                    _monoBehaviour.AttackableMove(_monoBehaviour._attackMove * _monoBehaviour.GetFacing());
                    _monoBehaviour.CurrentWaveGauge -= 10;
                    isPowerAttack = false;
                }
            }

        }
        public override void OnSLStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_monoBehaviour._clickcount == 1)
            {
                _monoBehaviour.MeleeAttackComoEnd();
            }

            if (animator.speed == 0.06f) animator.speed = 1.0f;
            _monoBehaviour._attack.DefultDamage();
            _monoBehaviour.DisableAttack();
        }
    }
}