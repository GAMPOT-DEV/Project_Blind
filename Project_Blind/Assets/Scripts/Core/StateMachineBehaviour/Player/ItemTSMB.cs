using UnityEngine;
using UnityEngine.Animations;

namespace Blind.Player
{
    public class ItemTSMB:SceneLinkedSMB<PlayerCharacter>
    {
        public override void OnSLStatePostEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex)
        {
            _monoBehaviour._moveVector.x = 0;
            _monoBehaviour._moveVector.y = 0;
            _monoBehaviour.ThrowItem();
            DataManager.Instance.DeleteBagItem(Define.BagItem.WaveStick);
        }
    }
}