using System;
using Cinemachine;
using UnityEngine;

namespace Blind 
{
    /// <summary>
    /// GameManager 클래스입니다. 게임의 전반적인 진행을 관리합니다.
    /// </summary>
    public class GameManager : Manager<GameManager>
    {
        [SerializeField] private AudioClip bgm;
        [SerializeField] private Transform SpawnPoint;
        public PlayerCharacter Player;
        private InputController _inputController; 
        protected override void Awake()
        {
            base.Awake();
            // 플레이어 할당하는 코드
            // 나중에 고치면 좋을듯
            Player = ResourceManager.Instance.Instantiate("Player2").GetComponent<PlayerCharacter>();
            Player.spawnPoint = SpawnPoint;
            Debug.Log(SpawnPoint);
            Player.gameObject.transform.position = SpawnPoint.position;
            Player.transform.SetParent(GameObject.Find("Entity").transform);
            GameObject.Find("CM Virtual Camera").GetComponent<CinemachineVirtualCamera>().Follow = Player.transform;
            _inputController = InputController.Instance;
            SoundManager.Instance.Play(bgm,Define.Sound.Bgm);
            var data = DataManager.Instance.PlayerCharacterDataValue;
            if (data != null)
            {
                Debug.Log(data.Hp.GetHP() + "체력");
                Player.SetPlayerValue(data);
            }

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

