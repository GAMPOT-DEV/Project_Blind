using System;
using UnityEngine;

namespace Blind
{
    public class SightUnit : MonoBehaviour
    {
        private void Update()
        {
            var ray = new Ray(transform.position + new Vector3(0,0,-1),new Vector3(0,0,1) * 10 );
            bool raycast = Physics.Raycast(ray, out var hit);

            SightController.Instance.DrawTexture(hit);
        }
    }
}