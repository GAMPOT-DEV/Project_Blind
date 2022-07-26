using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Blind
{
    public class SightUnit : MonoBehaviour
    {
        public int Range = 3;

        private void Awake()
        {
            if (SightController.Instance is not null)
                SightController.Instance.AssignUnit(this);
        }
    }
}