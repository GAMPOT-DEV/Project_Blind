using System;
using System.Collections.Generic;
using Blind.Abyss;
using UnityEngine;

namespace Blind
{
    public class AbyssSceneManager : Manager<AbyssSceneManager>
    {
        [SerializeField] private List<Stage> _stageInfo;
        private IEnumerator<Stage> currentStage;


        protected override void Awake()
        {
            base.Awake();
            currentStage = _stageInfo.GetEnumerator();
            currentStage.MoveNext();
            currentStage.Current!.Enable();
        }
        
        public void MoveNextStage()
        {
            if (currentStage.Current == null) return;
            var prev = currentStage.Current;
            if (currentStage.MoveNext())
            {
                prev.Disable();
                if (currentStage.Current != null) 
                    currentStage.Current.Enable();
            }
        }
        
    }
}