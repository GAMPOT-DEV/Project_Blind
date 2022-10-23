using UnityEngine;
using UnityEngine.Animations;

namespace Blind
{
    public class Pattern3SMB : SceneLinkedSMB<FirstBossEnemy>
    {
        public override void OnSLStateEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex) {
            _monoBehaviour.AttackInit(3,3,1);
            SoundManager.Instance.Play("장산범/비명2");
        }
        public override void OnSLStatePostEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex) {
            var wave = ResourceManager.Instance.Instantiate("MapObjects/Wave/WaveSense 2").GetComponent<BossWaveSense>();
            wave.transform.position = _monoBehaviour.ShoutePatternPosition.position;
            wave.StartSpread();
            _monoBehaviour._source.GenerateImpulse();
        }
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
        }
    }
}