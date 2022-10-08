using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class InteractionTest : InteractionAble
    {
        public string InteractionText;

        int _x = 7;
        int _y = 7;
        public Define.ScriptTitle ScriptTitle;
        public Define.BagItem BagItem;
        public Define.ClueItem ClueItem;

        protected override void Init(int x = 5, int y = 5)
        {
            base.Init(_x, _y);
        }
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<PlayerCharacter>() == null) return;
            // UI_TestInteraction를 WorldSpace로 띄운다.

            _ui = UIManager.Instance.ShowWorldSpaceUI<UI_Interaction>();
            _ui.SetPosition(gameObject.transform.position, Vector3.down * 1);
            (_ui as UI_Interaction).InteractionAction = InteractionAction;
        }
        protected override void OnTriggerExit2D(Collider2D collision)
        {
            if (_ui != null)
                _ui.CloseWorldSpaceUI();
        }
        public override void DoInteraction()
        {
            
        }
        private void InteractionAction()
        {
            _ui = UIManager.Instance.ShowWorldSpaceUI<UI_TestConversation>();
            _ui.Owner = gameObject;
            (_ui as UI_TestConversation).Title = ScriptTitle;
            (_ui as UI_TestConversation).BagItem = BagItem;
            (_ui as UI_TestConversation).ClueItem = ClueItem;
            _ui.SetPosition(gameObject.transform.position, Vector3.up * 3);

            PlayerCharacter player = FindObjectOfType<PlayerCharacter>();
            player.GetComponent<PlayerCharacter>().Talk();
        }
    }
}

