using System;
using UnityEngine;

namespace Blind.JangSanBum
{
    public class Point : MonoBehaviour
    {
        private Action _action;

        public void SetAction(Action action)
        {
            _action = action;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            _action();
            gameObject.SetActive(false);
        }
    }
}