using UnityEngine;
using UnityEngine.Animations;

namespace Blind
{
    public class ItemUsingSMB :SceneLinkedSMB<PlayerCharacter>
    {
        public override void OnSLStatePostEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex)
        {
            _monoBehaviour.HpHeal();
            DataManager.Instance.DeleteBagItem(Define.BagItem.Potion);
            _monoBehaviour._moveVector.x = 0f;
        }
        
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            _monoBehaviour.UpdateVelocity();
        }
    }
}