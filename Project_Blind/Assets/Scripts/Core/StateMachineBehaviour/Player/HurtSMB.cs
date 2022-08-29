using UnityEngine;
using UnityEngine.Animations;

namespace Blind
{
    public class HurtSMB: SceneLinkedSMB<PlayerCharacter>
    {
        public override void OnSLStateEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex) {
            _monoBehaviour.MeleeAttackComoEnd();
            _monoBehaviour._isHurtCheck = false;
            animator.speed = 2f;
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            _monoBehaviour.AirborneVerticalMovement(3f);
            _monoBehaviour.GroundedHorizontalMovement(false);
            if(_monoBehaviour._isHurtCheck)
                animator.Play("Hurt",-1,0);
        }
        public override void OnSLStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.speed = 1f;
            animator.ResetTrigger("Hurt");
        }
    }
}