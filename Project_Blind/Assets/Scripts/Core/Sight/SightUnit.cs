using System;
using UnityEngine;

namespace Blind
{
    public class SightUnit : MonoBehaviour
    {
        public int Range = 3;

        private void Awake()
        {
            SightController.Instance.AssignUnit(this);
        }
    }
}