using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    // TODO
    public class ConversationTest : InteractionAble
    {
        protected override void Init(int x = 3, int y = 3)
        {
            base.Init(x, y);
        }
        public override void DoInteraction(GameObject player)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnTriggerExit2D(Collider2D collision)
        {
            throw new System.NotImplementedException();
        }
    }
}

