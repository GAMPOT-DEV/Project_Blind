using UnityEngine;
using UnityEngine.Animations;

namespace Blind
{
    public class Pattern1SMB : SceneLinkedSMB<FirstBossEnemy>
    {
        public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _monoBehaviour.AttackInit(3, 3, 1);
            _monoBehaviour.gameObject.layer = 15;
            SoundManager.Instance.Play("장산범/바닥긁기공격", Define.Sound.Effect);
        }

        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            if (_monoBehaviour.CheckForPlayerDead())
            {
                _monoBehaviour.isPlayerDead = true;
            }
        }

        public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _monoBehaviour.gameObject.layer = 0;
        }
    }
}