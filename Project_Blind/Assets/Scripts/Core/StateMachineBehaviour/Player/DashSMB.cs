using UnityEngine;
using UnityEngine.Animations;
namespace Blind
{
    public class DashSMB :SceneLinkedSMB<PlayerCharacter>
    {
        public override void OnSLStateEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex)
        {
            _monoBehaviour.Dash();
        }
        public override void OnSLStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _monoBehaviour.DashCoolTime();
        }
    }
}