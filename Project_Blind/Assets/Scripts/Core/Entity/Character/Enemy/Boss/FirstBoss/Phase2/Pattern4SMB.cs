using UnityEngine;
using UnityEngine.Animations;

namespace Blind
{
    public class Pattern4SMB: SceneLinkedSMB<FirstBossEnemy>
    {
        public override void OnSLStateEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex) {
            _monoBehaviour.AttackInit(7,10,1);
            _monoBehaviour.Pattern4Start();
            Debug.Log("tlfgoehla");
        }
        public override void OnSLStatePostEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex) {
            if (_monoBehaviour.CheckForPlayerDead())
            {
                _monoBehaviour.isPlayerDead = true;
            }
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            
        }
    }
}