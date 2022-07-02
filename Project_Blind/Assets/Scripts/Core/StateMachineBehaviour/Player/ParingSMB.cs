using UnityEngine;
using UnityEngine.Animations;

namespace Blind
{
    public class ParingSMB: SceneLinkedSMB<PlayerCharacter>
    {
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _monoBehaviour.ParingObjCheck();
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            Debug.Log("paring 실행");
            if(_monoBehaviour.CheckEnemy())
                _monoBehaviour.EnemyStateCheck();
        }
    }
}