using UnityEngine;
using UnityEngine.Animations;

namespace Blind.Player
{
    public class ItemTSMB:SceneLinkedSMB<PlayerCharacter>
    {
        public override void OnSLStatePostEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex) {
            _monoBehaviour.ThrowItem();
            DataManager.Instance.DeleteBagItem(Define.BagItem.TestItem1);
        }
    }
}