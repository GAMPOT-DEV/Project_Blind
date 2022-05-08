using UnityEngine;
using UnityEngine.Animations;
namespace Blind
{
    /// <summary>
    /// 공중에 떠있을 때에 상태를 정의합니다.
    /// </summary>
    public class JumpingSMB : SceneLinkedSMB<PlayerCharacter>
    {
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            _monoBehaviour.UpdateJump();
            _monoBehaviour.AirborneVerticalMovement();
            _monoBehaviour.GroundedHorizontalMovement(true);
            _monoBehaviour.UpdateVelocity();
            _monoBehaviour.CheckForGrounded();
        }
    }
}