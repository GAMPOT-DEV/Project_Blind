using UnityEngine;
using UnityEngine.Animations;
namespace Blind
{
    public class DashSMB :SceneLinkedSMB<PlayerCharacter>
    {
        public override void OnSLStateEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex)
        {
            SoundManager.Instance.Play("Player/빠른 이동", Define.Sound.Effect);
            _monoBehaviour.GroundedHorizontalMovement(false);
            _monoBehaviour.StopDash();
            animator.speed = 2.5f;
            _monoBehaviour.gameObject.layer = 17;
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
            _monoBehaviour.gameObject.layer = 13;

        }
    }
}