using System.Collections.Generic;
using UnityEngine;

namespace Blind.Abyss
{
    public class Stage : MonoBehaviour
    {
        public List<Wall> Walls;
        
        protected virtual void Awake()
        {
            //
        }
        public virtual void Enable()
        {
            foreach (var wall in Walls)
            {
                wall.Enable();    
            }
        }

        public virtual void Disable()
        {
            foreach (var wall in Walls)
            {
                wall.Disable();    
            }
        }

        public void NextStage()
        {
            AbyssSceneManager.Instance.MoveNextStage();
        }
    }
}