using UnityEngine;
using UnityEngine.Animations;
using System.Collections.Generic;

namespace Blind
{
    public class GanrimSMB: SceneLinkedSMB<FirstBossEnemy>
    {
        public override void OnSLStateEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex) {
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            
        }
        public override void OnSLStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _monoBehaviour.StartCoroutine(_monoBehaviour.StartNextPhaseScreenFade());
        }
    }
}