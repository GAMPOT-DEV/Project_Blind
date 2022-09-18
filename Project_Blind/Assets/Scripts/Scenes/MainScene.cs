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

        }
        public override void Clear()
        {

        }
    }
}

