using UnityEngine;

namespace Blind
{
    public class ItemUsingSMB :SceneLinkedSMB<PlayerCharacter>
    {
        public override void OnSLStatePostEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex)
        {
            _monoBehaviour.HpHeal();
            DataManager.Instance.DeleteBagItem(Define.BagItem.Potion);
        }
    }
}