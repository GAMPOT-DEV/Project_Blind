using UnityEngine;
using UnityEngine.Animations;
using System.Collections.Generic;
using System.Collections;

namespace Blind
{
    public class BossDeadSMB : SceneLinkedSMB<FirstBossEnemy>
    {
        public override void OnSLStateEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex) {
            SoundManager.Instance.Play("장산범/Dead",Define.Sound.Effect);
            _monoBehaviour.EndStart();
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            
        }
        
        public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log("t실행됨");
        }
    }
}