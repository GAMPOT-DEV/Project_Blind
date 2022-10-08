using UnityEngine;
using UnityEngine.Animations;
namespace Blind
{
    /// <summary>
    /// 공중에 떠있을 때에 상태를 정의합니다.
    /// </summary>
    public class JumpingSMB : SceneLinkedSMB<PlayerCharacter>
    {
        public override void OnSLStateEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex) {
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            _monoBehaviour.CheckForGrounded();
            _monoBehaviour.WaveSensePress();
            _monoBehaviour.UpdateJump();
            _monoBehaviour.AirborneVerticalMovement(_monoBehaviour.gravity);
            _monoBehaviour.GroundedHorizontalMovement(true);
            _monoBehaviour.UpdateFacing();
            if(_monoBehaviour.CheckForParing())
                _monoBehaviour.Paring();
            if (_monoBehaviour.CheckForDash())
            {
                _monoBehaviour.DashStart();
            }
            if (_monoBehaviour.CheckForAttack())
            {
                _monoBehaviour.MeleeAttack();
                _monoBehaviour._lastClickTime = Time.time;
                _monoBehaviour._clickcount++;
                _monoBehaviour._clickcount = Mathf.Clamp(_monoBehaviour._clickcount, 0, 4);
            }
            if(_monoBehaviour.CheckForDeed())
            {
                _monoBehaviour.Deed();
            }

            if (_monoBehaviour.CheckForItemT())
            {
                _monoBehaviour.ItemT();
            }
        }
    }
}