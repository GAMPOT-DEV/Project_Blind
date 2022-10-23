using UnityEngine;
using UnityEngine.Animations;

namespace Blind
{
    public class BossDeadSMB : SceneLinkedSMB<FirstBossEnemy>
    {
        public override void OnSLStateEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex) {
            SoundManager.Instance.Play("장산범/Dead",Define.Sound.Effect);
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            if (_monoBehaviour.CheckForDead())
            {
                _monoBehaviour.Dead();
            }
        }
    }
}