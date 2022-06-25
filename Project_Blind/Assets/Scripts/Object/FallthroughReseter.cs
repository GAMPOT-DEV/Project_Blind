using System.Collections;
using UnityEngine;

namespace Blind
{
    public class FallthroughReseter : MonoBehaviour
    {
        public void StarFall(PlatformEffector2D effector)
        {
            StartCoroutine(FallCoroutine(effector));
        }

        IEnumerator FallCoroutine(PlatformEffector2D effector)
        {
            
            int playerLayerMask = 1 << LayerMask.NameToLayer("Units");

            effector.colliderMask &= ~playerLayerMask;
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            yield return new WaitForSeconds(0.7f);

            effector.colliderMask |= playerLayerMask;
            gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
            
            Destroy(this);
        }
    }
}