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
                var position = transform.parent.localPosition;
                position = new Vector3((float)_face*Math.Abs(position.x),position.y,position.z);
                transform.parent.localPosition = position;
                var localScale = transform.parent.localScale;
                localScale = new Vector3(-(float)_face*Math.Abs(localScale.x),localScale.y,localScale.z);
                transform.parent.localScale = localScale;
            }
            _particleSystem.Play();
        }
    }
}