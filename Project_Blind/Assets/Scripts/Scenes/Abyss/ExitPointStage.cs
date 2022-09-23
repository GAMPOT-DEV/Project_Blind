using System;
using UnityEngine;

namespace Blind.Abyss
{
    public class ExitPointStage : Stage
    {
        public GameObject ExitPoint;

        protected virtual void Awake()
        {
            ExitPoint.GetComponent<ExitPoint>().stage = this;
        }

        public override void Disable()
        {
            base.Disable();
            ExitPoint.GetComponent<ExitPoint>().Disable();
        }
    }
    
}