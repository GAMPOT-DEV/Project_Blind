using UnityEngine;
using UnityEngine.Animations;


namespace Blind
{
    public class DefultSMB : SceneLinkedSMB<SecondPhase>
    {
        public override void OnSLStateEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex) {
            _monoBehaviour.StartPattern(_monoBehaviour.Range());
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            if (_monoBehaviour.Hp.GetHP() <= 0)
            {
                _monoBehaviour.Dead();
            }
        }
    }
}