using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class TestPlayer : MonoBehaviour
    {
        public TestFOW fogOfWar;
        public Transform secondaryFogOfWar;
        [Range(0, 5)]
        public float sightDistance;
        public float checkInterval;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(CheckFogOfWar(checkInterval));
            secondaryFogOfWar.localScale = new Vector2(sightDistance, sightDistance) * 10f;
        }

        // Update is called once per frame
        private IEnumerator CheckFogOfWar(float checkInterval)
        {
            while (true)
            {
                fogOfWar.MakeHole(transform.position, sightDistance);
                yield return new WaitForSeconds(checkInterval);
            }
        }
    }
}

