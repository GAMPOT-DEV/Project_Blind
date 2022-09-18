using System;
using UnityEngine;

namespace Blind.Abyss
{
    [RequireComponent(typeof(Collider2D))]
    public class Wall : MonoBehaviour
    {

        private Collider2D _collider;

        public void Awake()
        {
            _collider = GetComponent<Collider2D>();
            Disable();
        }

        public void Enable()
        {
            _collider.enabled = true;
        }

        public void Disable()
        {
            _collider.enabled = false;
        }
    }
}