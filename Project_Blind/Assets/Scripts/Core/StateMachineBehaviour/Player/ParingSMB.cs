using UnityEngine;
using UnityEngine.Animations;

namespace Blind
{
    public class ParingSMB: SceneLinkedSMB<PlayerCharacter>
    {
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _monoBehaviour.EnableParing();
            Debug.Log("응애");
            _monoBehaviour.transform.GetChild(1).GetChild(9).GetComponent<AttackFX>().Play(_monoBehaviour.GetFacing());
            if (_monoBehaviour.isJump)
            {
                _monoBehaviour.DieStopVector(Vector2.zero);
            }

            if (_monoBehaviour.isParingCheck)
            {
                animator.speed = 2f;
            }
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            _monoBehaviour.UpdateVelocity();

            if(_monoBehaviour.isJump) _monoBehaviour.GroundedHorizontalMovement(false);
            else
            {
                _monoBehaviour.GroundedHorizontalMovement(true);
                _monoBehaviour.AirborneVerticalMovement(_monoBehaviour.gravity);
                _monoBehaviour.UpdateJump();
                _monoBehaviour.UpdateVelocity();
            }
            if (_monoBehaviour.isParingCheck)
            {
            }
        }
        
        public override void OnSLStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _monoBehaviour.DisableParing();
            animator.speed = 1f;
            _monoBehaviour.isParingCheck = false;
        }
    }
}