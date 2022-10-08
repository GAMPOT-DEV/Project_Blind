using UnityEngine;
using UnityEngine.Animations;

namespace Blind
{
    public class Pattern3SMB : SceneLinkedSMB<FirstBossEnemy>
    {
        public override void OnSLStateEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex) {
            _monoBehaviour.AttackInit(3,3,3);
        }
        public override void OnSLStatePostEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex) {
            var wave = ResourceManager.Instance.Instantiate("MapObjects/Wave/WaveSense 2").GetComponent<WaveSense>();
            wave.transform.position = _monoBehaviour.ShoutePatternPosition.position;
            wave.StartSpread();
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            
        }
    }
}