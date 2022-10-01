using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class Clue : InteractionAble
    {
        public Define.ClueItem clueItem;
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
                DataManager.Instance.AddClueItem(clueItem);
                gameObject.SetActive(false);
            }

        }

        protected override void OnTriggerExit2D(Collider2D collision)
        {
        }

        public override void DoInteraction()
        {
        }

    }


}

