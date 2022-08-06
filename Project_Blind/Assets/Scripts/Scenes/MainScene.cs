using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class MainScene : BaseScene
    {
        protected override void Init()
        {
            base.Init();
            SceneType = Define.Scene.MainScene;
            UIManager.Instance.ShowSceneUI<UI_MainScene>();

            //DataManager.Instance.GameData.clueSlotInfos.Add(new ClueInfo() { slot = 0, itemId = 1 });
            //DataManager.Instance.GameData.clueSlotInfos.Add(new ClueInfo() { slot = 3, itemId = 5 });
        }
        public override void Clear()
        {

        }
    }
}

