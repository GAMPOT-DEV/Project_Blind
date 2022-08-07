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

            DataManager.Instance.LoadGameData();
            //DataManager.Instance.GameData.clueSlotInfos.Add(new ClueInfo() { slot = 0, itemId = 1 });
            //DataManager.Instance.GameData.clueSlotInfos.Add(new ClueInfo() { slot = 2, itemId = 7 });
            //DataManager.Instance.GameData.clueSlotInfos.Add(new ClueInfo() { slot = 4, itemId = 5 });
            //DataManager.Instance.GameData.clueSlotInfos.Add(new ClueInfo() { slot = 6, itemId = 3 });
        }
        public override void Clear()
        {

        }
    }
}

