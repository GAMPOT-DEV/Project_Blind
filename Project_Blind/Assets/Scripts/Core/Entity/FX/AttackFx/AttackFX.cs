using System;
using UnityEngine;

namespace Blind
{
    [RequireComponent(typeof(ParticleSystem))]
    public abstract class AttackFX : MonoBehaviour
    {
        private Facing _face = Facing.Left;
        private Character _character;

        protected virtual void Awake()
        {
            _character = transform.parent.GetComponent<Character>();
        }

        public virtual void Play(Facing face)
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
        }
    }
}