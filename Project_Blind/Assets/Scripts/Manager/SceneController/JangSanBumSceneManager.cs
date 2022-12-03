using System;
using UnityEngine;

namespace Blind.JangSanBum
{
    public class JangSanBumSceneManager : Manager<JangSanBumSceneManager>
    {
        [SerializeField] private Point _entryPoint;
        [SerializeField] private Point _exitPoint;
        [SerializeField] private GameObject wall;
        [SerializeField] private FirstBossEnemy _firstBossEnemy;
        private GameObject _firstBossHpBar;
        private bool isBossBarCheck = false;
        protected override void Awake()
        {
            base.Awake();
            _entryPoint.SetAction(() =>
            {
                wall.SetActive(true);
                _firstBossEnemy.gameObject.SetActive(true);
                if(!isBossBarCheck) _firstBossHpBar = ResourceManager.Instance.Instantiate("UI/Normal/UI_BossHp");
                else _firstBossHpBar.SetActive(true);
                isBossBarCheck = true;
                _firstBossEnemy.Play();
            });
            _exitPoint.SetAction(() =>
            {
                wall.SetActive(false);
            });
        }

        public void Initialize()
        {
            wall.SetActive(true);
            _entryPoint.gameObject.SetActive(true);
            _exitPoint.gameObject.SetActive(true);
            _firstBossEnemy.Reset();
        }

        public void Reset()
        {
            wall.SetActive(false);
            _entryPoint.gameObject.SetActive(true);
            _exitPoint.gameObject.SetActive(true);
            _firstBossHpBar.SetActive(false);
        }
    }
}