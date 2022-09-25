using UnityEngine;

namespace Blind
{
    [RequireComponent(typeof(ParticleSystem))]
    public class AttackParticleFx : AttackFX
    {
        private ParticleSystem _particleSystem;

        protected override void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            _particleSystem.Stop();
            base.Awake();
        }

        public override void Play(Facing face)
        {
            base.Play(face);
            _particleSystem.Play();
        }
    }
}