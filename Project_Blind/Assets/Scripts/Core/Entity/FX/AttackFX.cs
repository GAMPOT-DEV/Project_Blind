using System;
using UnityEngine;

namespace Blind
{
    [RequireComponent(typeof(ParticleSystem))]
    public class AttackFX : MonoBehaviour
    {
        private Facing _face = Facing.Left;
        private ParticleSystem _particleSystem;
        private Character _character;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            _character = transform.parent.GetComponent<Character>();
        }

        public void Play(Facing face)
        {
            if (face != _face)
            {
                _face = face;
                var position = transform.localPosition;
                position = new Vector3(-position.x,position.y,position.z);
                transform.localPosition = position;
                var localScale = transform.localScale;
                localScale = new Vector3(-localScale.x,localScale.y,localScale.z);
                transform.localScale = localScale;
            }
            _particleSystem.Play();
        }
    }
}