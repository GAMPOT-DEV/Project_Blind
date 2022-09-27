using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class MainScene : BaseScene
    {
        [SerializeField] private AudioClip bgm;
        protected override void Init()
        {
            base.Init();
            SoundManager.Instance.Play(bgm,Define.Sound.Bgm);
            SceneType = Define.Scene.MainScene;
            UIManager.Instance.ShowSceneUI<UI_MainScene>();

            DataManager.Instance.LoadGameData();
            DataManager.Instance.AddClueItem(Define.ClueItem.TestClue1);

            UI_ScreenConversation ui = UIManager.Instance.ShowNormalUI<UI_ScreenConversation>();
            ui.SetName("테스트");
            ui.SetScriptTitle(Define.ScriptTitle.Test1);
        }
        public override void Clear()
        {

        }
    }
}

