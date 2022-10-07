using UnityEngine;
using UnityEngine.Animations;

namespace Blind
{
    public class Pattern4SMB: SceneLinkedSMB<FirstBossEnemy>
    {
        public override void OnSLStateEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex) {
            _monoBehaviour.AttackInit(3,3,3);
            _monoBehaviour.Pattern3Start();
        }
        public override void OnSLStatePostEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex) {
            
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            
        }
    }
}