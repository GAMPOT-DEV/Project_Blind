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

        protected override void Awake()
        {
            base.Awake();
            _entryPoint.SetAction(() =>
            {
                wall.SetActive(true);
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
        }
    }
}