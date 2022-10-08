using UnityEngine;
using UnityEngine.Animations;

namespace Blind
{
    public class FlyParingSMB: SceneLinkedSMB<PlayerCharacter>
    {
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _monoBehaviour.EnableParing();

            if (_monoBehaviour.isParingCheck)
            {
                animator.speed = 2f;
            }
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            
            _monoBehaviour.CheckForGrounded();
            _monoBehaviour.GroundedHorizontalMovement(true);
            _monoBehaviour.AirborneVerticalMovement(_monoBehaviour.gravity);
            _monoBehaviour.UpdateJump();
            _monoBehaviour.UpdateVelocity();

        }
        
        public override void OnSLStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _monoBehaviour.DisableParing();
            animator.speed = 1f;
            _monoBehaviour.isParingCheck = false;
        }
    }
}