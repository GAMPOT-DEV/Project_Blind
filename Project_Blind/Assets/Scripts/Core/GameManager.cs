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
        protected override void Awake()
        {
            base.Awake();
        }

        private void Update()
        {
            
        }

        private void FixedUpdate()
        {
            Player.OnFixedUpdate();
        }
        
    }
}

