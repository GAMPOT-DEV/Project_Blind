using UnityEngine;
using UnityEngine.Animations;
namespace Blind
{
    public class MeleeAttackComboSMB: SceneLinkedSMB<PlayerCharacter>
    {
        public override void OnSLStateEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex) {
            _monoBehaviour.enableAttack();
            _monoBehaviour.AttackableMove(_monoBehaviour._attackMove * _monoBehaviour.GetFacing());
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            _monoBehaviour.GroundedHorizontalMovement(false);
            if (_monoBehaviour.CheckForAttack())
            {
                _monoBehaviour._lastClickTime = Time.time;
                _monoBehaviour._clickcount++;
                _monoBehaviour._clickcount = Mathf.Clamp(_monoBehaviour._clickcount, 0, 4);
            }
            if (_monoBehaviour.CheckForAttackTime())
                _monoBehaviour._clickcount = 0; 
            if (_monoBehaviour._clickcount >= 2)
                _monoBehaviour.MeleeAttackCombo1();
            if(_monoBehaviour._clickcount == 0)
                _monoBehaviour.MeleeAttackComoEnd();
            
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