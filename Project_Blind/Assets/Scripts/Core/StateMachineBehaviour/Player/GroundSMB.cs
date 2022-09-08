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
            
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            _monoBehaviour.CheckForGrounded();
            _monoBehaviour.WaveSensePress();
            _monoBehaviour.Jump();
            _monoBehaviour.GroundedHorizontalMovement(true);
            _monoBehaviour.UpdateVelocity();
            _monoBehaviour.UpdateFacing();
            
            if (_monoBehaviour.CheckForDash())
            {
                _monoBehaviour.DashStart();
            }
            
            if (_monoBehaviour.CheckForFallInput())
                _monoBehaviour.MakePlatformFallthrough();
            if(_monoBehaviour.CheckForParing())
                _monoBehaviour.Paring();
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

            if (_monoBehaviour.CheckForSkill())
            {
                _monoBehaviour.Skill();
            }
        }
        
    }
}