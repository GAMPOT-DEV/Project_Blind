using UnityEngine;
using UnityEngine.Animations;

namespace Blind
{
    public class DeadSMB: SceneLinkedSMB<PlayerCharacter>
    {
        public override void OnSLStateEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex) {
            _monoBehaviour.DieStopVector(Vector2.zero);
            _monoBehaviour._characterController2D.isDie = true;
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            Debug.Log("Test");
        }
    }
}