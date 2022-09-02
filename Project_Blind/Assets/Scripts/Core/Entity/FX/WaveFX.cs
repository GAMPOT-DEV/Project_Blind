using System;
using UnityEngine;

namespace Blind
{
    [RequireComponent(typeof(ParticleSystem))]
    public class WaveFX : MonoBehaviour
    {
        private ParticleSystem _particleSystem;
        private Vector3 _lastPos;
        private ParticleSystem.EmissionModule _emission;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            _emission = _particleSystem.emission;
        }

        private void FixedUpdate()
        {
            CheckPosition();
        }

        private void CheckPosition()
        {
            if (_lastPos != transform.parent.localPosition)
            {
                _lastPos = transform.parent.localPosition;
                _emission.enabled = true;
            }
            else
            {
                _emission.enabled = false;
            }
        }
    }
}