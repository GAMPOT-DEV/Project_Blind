using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class BigTree : InteractionAble
    {
        public override void DoInteraction()
        {
            DataManager.Instance.CaveOpen();
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
                StartCoroutine(CheckWaveSpread());
            }
        }

        protected override void OnTriggerExit2D(Collider2D collision)
        {
        }

        IEnumerator CheckWaveSpread()
        {
            DoInteraction();
            yield return null;
        }
    }
}

