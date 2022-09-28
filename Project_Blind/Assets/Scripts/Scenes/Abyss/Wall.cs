using System;
using UnityEngine;

namespace Blind.Abyss
{
    [RequireComponent(typeof(Collider2D))]
    public class Wall : MonoBehaviour
    {

        private Collider2D _collider;
        private GameObject cloud;

        public void Awake()
        {
            _collider = GetComponent<Collider2D>();
            if(transform.GetChild(0) != null)
            {
                cloud = transform.GetChild(0).gameObject;
            }
            else cloud = null;
            Disable();
        }

        public void Enable()
        {
            _collider.enabled = true;
            if(cloud != null)
            {
                cloud.SetActive(true);
            }
        }

        public void Disable()
        {
            _collider.enabled = false;
            if(cloud !=null)
            {
                cloud.SetActive(false);
            }
        }
    }
}