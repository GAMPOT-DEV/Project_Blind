using System;
using UnityEngine;

namespace Blind.Abyss
{
    public class ExitKillStage : ExitPointStage
    {
        [SerializeField] private EnemyCharacter _enemyCharacter;
        [SerializeField] private Wall _leaveWall;

        protected override void Awake()
        {
            _enemyCharacter.gameObject.SetActive(false);
            _enemyCharacter.DeathCallback = () =>
            {
                Debug.Log("deathCallBack");
                _leaveWall.Disable();
            };
            base.Awake();
        }

        public override void Enable()
        {
            base.Enable();
            _enemyCharacter.gameObject.SetActive(true);
        }
        
    }
}