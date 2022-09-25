using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Blind
{
    public class InvisibleFloor : MonoBehaviour
    {
        private SpriteRenderer _renderer;
        private Collider2D _collider;
        private ShadowCaster2D _shadowCaster;
        private void Awake()
        {
            _renderer = gameObject.GetComponent<SpriteRenderer>();
            _renderer.enabled = false;
            _collider = gameObject.GetComponent<Collider2D>();
            _collider.enabled = false;
            _shadowCaster = gameObject.GetComponent<ShadowCaster2D>();
            _shadowCaster.enabled = false;
        }

        public void SetVisible()
        {
            _renderer.enabled = true;
            _collider.enabled = true;
            _shadowCaster.enabled = true;
        }

    }
}


