using UnityEngine;
using UnityEngine.Animations;


namespace Blind
{
    public class DefultSMB : SceneLinkedSMB<FirstBossEnemy>
    {
        public override void OnSLStateEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex) {
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            if (_monoBehaviour.CheckForDead())
            {
                _monoBehaviour.Dead();
            }
            
        }
        public override void OnSLStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (GameManager.Instance.Player.isCheckDead)
            {
                _monoBehaviour.Reset();
            }
        }
    }
}