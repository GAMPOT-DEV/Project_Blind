using UnityEngine;
using UnityEngine.Animations;

namespace Blind
{
    public class Pattern2SMB: SceneLinkedSMB<FirstBossEnemy>
    {
        public override void OnSLStateEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex) {
            _monoBehaviour.AttackInit(7,7,1);
            SoundManager.Instance.Play("장산범/물기공격");
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            
        }
    }
}