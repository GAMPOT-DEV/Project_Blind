using UnityEngine;
using UnityEngine.Animations;

namespace Blind
{
    public class ParingSMB: SceneLinkedSMB<PlayerCharacter>
    {
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _monoBehaviour.EnableParing();
            _monoBehaviour.DieStopVector(Vector2.zero);

            if (_monoBehaviour.isParingCheck)
            {
                _monoBehaviour._paring.gameObject.transform.position =
                    new Vector2(_monoBehaviour._paring.gameObject.transform.position.x * -1,
                        _monoBehaviour._paring.gameObject.transform.position.y);
                animator.speed = 2f;
            }
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            _monoBehaviour.GroundedHorizontalMovement(false);
            if (_monoBehaviour.isParingCheck)
            {
                animator.speed = 2f;
            }
        }
        
        public override void OnSLStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Time.timeScale = 1f;
            _monoBehaviour.DisableParing();
            animator.speed = 1f;
            _monoBehaviour.isParingCheck = false;
        }
    }
}