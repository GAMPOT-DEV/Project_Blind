using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind.Abyss
{
    public class ExitParringStage : ExitPointStage
    {
        [SerializeField] private Wall _leaveWall;
        private void Awake()
        {
            base.Awake();
        }
    }
}

