using Blind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameScene Ŭ�����Դϴ�. GameScene���� �Ͼ�� �������� ���⼭ �����ϸ� ���� �� �����ϴ�.
/// </summary>

namespace Blind
{
    public class GameScene : BaseScene
    {
        UI_GameScene _sceneUI;
        protected override void Init()
        {
            base.Init();

            SceneType = Define.Scene.GameScene;


            // TEST CODE
            #region TEST CODE

            _sceneUI = UIManager.Instance.ShowSceneUI<UI_GameScene>("UI_GameScene_Test");

            GameObject _gameScenePoolRoot = new GameObject() { name = "@GameScenePoolRoot" };

            List<GameObject> _testPools = new List<GameObject>();
            for (int i = 0; i < 20; i++)
            {
                _testPools.Add(ResourceManager.Instance.Instantiate("TestPooling", _gameScenePoolRoot.transform));
            }
            for (int i = 0; i < 10; i++)
            {
                ResourceManager.Instance.Destroy(_testPools[i]);
            }

            #endregion
        }

        public override void Clear()
        {

        }
    }
}

