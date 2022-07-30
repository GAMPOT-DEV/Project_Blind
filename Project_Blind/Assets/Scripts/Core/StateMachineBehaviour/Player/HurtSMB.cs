using UnityEngine;
using UnityEngine.Animations;

namespace Blind
{
    public class HurtSMB: SceneLinkedSMB<PlayerCharacter>
    {
        public override void OnSLStateEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex) {
            _monoBehaviour.HurtMove(_monoBehaviour._hurtMove * (-_monoBehaviour.GetFacing()));
            _monoBehaviour.MeleeAttackComoEnd();
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            _monoBehaviour.AirborneVerticalMovement();
            _monoBehaviour.GroundedHorizontalMovement(false);
        }
    }
}