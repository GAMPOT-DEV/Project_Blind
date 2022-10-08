using System;
using UnityEngine;

namespace Blind
{
    public class LodingHub: Singleton<LodingHub>
    {
        public  TransitionPoint point;
        public void Awake()
        {
            
        }

        public void StartNextScene(TransitionPoint point)
        {
            this.point = point;
        }

        public void NextScene()
        {
            Debug.Log("실행돔..");
            Debug.Log(this.point.newSceneName);
            SceneController.TransitionToScene(point);
        }
    }
}