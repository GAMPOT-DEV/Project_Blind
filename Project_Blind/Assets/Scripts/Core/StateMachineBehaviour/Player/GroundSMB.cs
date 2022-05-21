using UnityEngine;
using UnityEngine.Animations;
namespace Blind
{
    /// <summary>
    /// 땅에 닿았을 때 상태를 정의합니다.
    /// </summary>
    public class GroundSMB : SceneLinkedSMB<PlayerCharacter>
    {
        public override void OnSLStateEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex) {
            Debug.Log("땅");
            _monoBehaviour.setJumping(false);
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            _monoBehaviour.Jump();
            _monoBehaviour.GroundedVerticalMovement();
            _monoBehaviour.GroundedHorizontalMovement(true);
            _monoBehaviour.UpdateVelocity();
            _monoBehaviour.CheckForGrounded();
            _monoBehaviour.Dash();
            _monoBehaviour.WaveSensePress();
            _monoBehaviour.UpdateFacing();
        }
        
    }
}