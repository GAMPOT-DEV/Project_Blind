using System;
using UnityEngine;

namespace Blind.Abyss
{

    public class ExitInputStage : Stage 
    {
        public GameObject ExitPoint;
        [SerializeField] private Wall _leaveWall;
        protected override void Awake()
        {
            base.Awake();
            ExitPoint.GetComponent<ExitPoint>().stage = this;
            _leaveWall.Enable();
        }
        void Update()
        {
            if (InputController.Instance.Wave.Down)
            {
                _leaveWall.Disable();
            }
        }
        public override void Disable()
        {
            base.Disable();
            ExitPoint.GetComponent<ExitPoint>().Disable();
        }
    }
}
