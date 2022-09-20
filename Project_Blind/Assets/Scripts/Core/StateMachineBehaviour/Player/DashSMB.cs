using UnityEngine;
using UnityEngine.Animations;
namespace Blind
{
    public class DashSMB :SceneLinkedSMB<PlayerCharacter>
    {
        public override void OnSLStateEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex)
        {
            SoundManager.Instance.Play("주인공 대쉬", Define.Sound.Effect);
            _monoBehaviour.GroundedHorizontalMovement(false);
            _monoBehaviour.StopDash();
            animator.speed = 2.5f;
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            _monoBehaviour.GroundedHorizontalMovement(false);
            _monoBehaviour.StopDash();
        }
        public override void OnSLStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.speed = 1f;
        }
    }
}