using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class FallthroughReseter : MonoBehaviour
    {
        // Start is called before the first frame update
        public void StarFall(PlatformEffector2D effector)
        {
            StartCoroutine(FallCoroutine(effector));
        }

        IEnumerator FallCoroutine(PlatformEffector2D effector)
        {
            effector.surfaceArc = -180;

            yield return new WaitForSeconds(1.5f);

            effector.surfaceArc = 180;

            Destroy(this);
        }
    }
}
