using System.Collections.Generic;
using UnityEngine;

namespace Blind.Abyss
{
    public class Stage : MonoBehaviour
    {
        public List<GameObject> Walls;
        
        protected virtual void Awake()
        {
            
        }
        public void Enable()
        {
            foreach (var wall in Walls)
            {
                wall.GetComponent<Wall>().Enable();    
            }
        }

        public virtual void Disable()
        {
            foreach (var wall in Walls)
            {
                wall.GetComponent<Wall>().Disable();    
            }
        }

        public void NextStage()
        {
            AbyssSceneManager.Instance.MoveNextStage();
        }
    }
}