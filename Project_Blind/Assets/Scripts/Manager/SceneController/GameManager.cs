using System;
using UnityEngine;

namespace Blind 
{
    /// <summary>
    /// GameManager 클래스입니다. 게임의 전반적인 진행을 관리합니다.
    /// </summary>
    public class GameManager : Manager<GameManager>
    {
        [SerializeField] public PlayerCharacter Player;
        private InputController _inputController;
        protected override void Awake()
        {
            base.Awake();
            _inputController = InputController.Instance;

            //UI_TestConversation ui = UIManager.Instance.ShowWorldSpaceUI<UI_TestConversation>();
            //ui.Title = "Test2";
            //ui.Owner = null;
            //ui.SetPosition(Player.transform.position , Vector3.right * 30);
        }
        protected void FixedUpdate()
        {
            if(Player != null)
            {
                Player.OnFixedUpdate();
            }
        }
    }
}

