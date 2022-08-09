using UnityEngine;
using UnityEngine.Animations;
namespace Blind
{
    public class MeleeAttackComboSMB: SceneLinkedSMB<PlayerCharacter>
    {
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            
        }

        public override void OnSLStatePostEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex) {
            _monoBehaviour.enableAttack();
            _monoBehaviour.AttackableMove(_monoBehaviour._attackMove * _monoBehaviour.GetFacing());
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            if (!_monoBehaviour.isJump)
            {
                _monoBehaviour.UpdateJump();
                _monoBehaviour.AirborneVerticalMovement();
                _monoBehaviour.AirborneHorizontalMovement();
                _monoBehaviour.CheckForGrounded();
            }
            _monoBehaviour.GroundedHorizontalMovement(false);
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
            if (_monoBehaviour.CheckForPowerAttack())
            {
                animator.speed = 0.5f;
            }

            if (_monoBehaviour.CheckForUpKey())
            {
                animator.speed = 1.0f;
            }

        }
        public override void OnSLStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_monoBehaviour._clickcount == 1)
            {
                _monoBehaviour.MeleeAttackComoEnd();
            }
            _monoBehaviour.DisableAttack();
        }
    }
}