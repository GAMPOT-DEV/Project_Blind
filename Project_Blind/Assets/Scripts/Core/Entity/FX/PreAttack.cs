using System.Collections;
using UnityEngine;


namespace Blind
{
    [RequireComponent(typeof(ParticleSystem))]
    public class PreAttack : MonoBehaviour
    {
        private ParticleSystem _particleSystem;
        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        public void Play()
        {
            StartCoroutine(_play());
        }

        private IEnumerator _play()
        {
            _particleSystem.Play();
            yield return new WaitForSeconds(1f);
            _particleSystem.Stop();
        }
    }
}
