using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Blind
{
    [RequireComponent(typeof(MeshRenderer))]
    public class SightController : SingletonDontCreate<SightController>
    {
        private SpriteRenderer _spriteRenderer;
        private MeshRenderer _meshRenderer;
        private RenderTexture _rt;
        private void Awake()
        {
            base.Awake();
            var resolution = UIManager.Instance.Resolution;
            _rt = new RenderTexture(resolution.width, resolution.height, 16);
            _meshRenderer.material.mainTexture = _rt;
        }

        public void AssignUnit(SightUnit sightUnit)
        {
            
        }
        
    }
}
