using System;
using System.Collections.Generic;
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

        public Transform Point = null;

        private InputController _inputController;
        private GameObject Enemy;
        CrowdEnemyCharacter[] enemyArr;
        protected override void Awake()
        {
            base.Awake();
            // 플레이어 할당하는 코드
            // 나중에 고치면 좋을듯
            
            Player = ResourceManager.Instance.Instantiate("Player3").GetComponent<PlayerCharacter>();
            Debug.Log(Player);
            Enemy = GameObject.Find("Enemy");
            // 적 캐릭터 리스트 할당
            enemyArr = Enemy.GetComponentsInChildren<CrowdEnemyCharacter>();

            //DataManager.Instance.LoadGameData();
            
        }

        public void Start()
        {
            Debug.Log("dd");
            Player.spawnPoint = SpawnPoint;
            Player.transform.SetParent(GameObject.Find("Entity").transform);
            GameObject.Find("CM Virtual Camera").GetComponent<CinemachineVirtualCamera>().Follow = Player.transform;
            _inputController = InputController.Instance;
            SoundManager.Instance.Play(bgm, Define.Sound.Bgm);
            var data = DataManager.Instance.PlayerCharacterDataValue;
            if (data != null)
            {
                Player.SetPlayerValue(data);
            }
            else
            {
                Player.transform.position = SpawnPoint.position;
            }

        }

        protected void FixedUpdate()
        {
            if (Player != null)
            {
                Player.OnFixedUpdate();
            }
        }


        public void ResetStage()
        {
            for (int i = 0; i < enemyArr.Length; i++)
            {
                enemyArr[i].Reset();
            }
        }

        public Transform GetTransform()
        {
            return SpawnPoint.transform;
        }

        public void SetSpawnPoint(Transform newPoint)
        {
            SpawnPoint = newPoint;
        }
    }
}

