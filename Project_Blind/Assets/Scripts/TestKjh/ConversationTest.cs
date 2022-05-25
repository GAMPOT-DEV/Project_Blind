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

        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<PlayerCharacter>() == null) return;
            _ui = UIManager.Instance.ShowWorldSpaceUI<UI_TestConversation>();
            _ui.Owner = gameObject;
            (_ui as UI_TestConversation).Title = "Test1";
            _ui.SetPosition(gameObject.transform.position, Vector3.up * 3);
        }

        protected override void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<PlayerCharacter>() == null) return;
            _ui.CloseWorldSpaceUI();
        }
    }
}

