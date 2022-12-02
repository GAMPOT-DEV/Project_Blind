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

            // 데이터 초기화
            DataManager.Instance.ClearBagData();
            DataManager.Instance.ClearClueData();
            DataManager.Instance.ClearTalismanData();
            DataManager.Instance.SubMoney(DataManager.Instance.GetMoney());

            DataManager.Instance.AddClueItem(Define.ClueItem.TestClue1);

            SoundManager.Instance.Play(bgm, Define.Sound.Bgm);
        }
        public override void Clear()
        {

        }
    }
}

