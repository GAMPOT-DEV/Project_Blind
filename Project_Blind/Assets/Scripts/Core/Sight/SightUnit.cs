using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Blind
{
    public class SightUnit : MonoBehaviour
    {
        [SerializeField]
        private int range = 3;

        private void Awake()
        {
            if (SightController.Instance is not null)
                SightController.Instance.AssignUnit(this);
        }

        public List<TilePos> GetRangeTiles(TilePos center)
        {
            var res = new List<TilePos>();
            for (var i = 0; i < range * 2; i++)
            {
                for (var j = 0; j < range * 2; j++)
                {
                    
                }
            }
            
            return res;
        }
    }
}