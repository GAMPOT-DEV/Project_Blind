using UnityEngine;
using UnityEngine.Animations;

namespace Blind
{
    public class Pattern1SMB : SceneLinkedSMB<SecondPhase>
    {
        public override void OnSLStateEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex) {
            
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            
        }
    }
}