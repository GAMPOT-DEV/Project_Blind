using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

namespace Blind
{
    // TODO
    public class ConversationTest : InteractionAble
    {
        public Define.ScriptTitle ScriptTitle;
        public Define.BagItem BagItem;
        public Define.ClueItem ClueItem;
        public Define.ObjectState _state;
        public GameObject _player = null;
        private Define.ObjectType _objType;
        protected override void Init(int x = 3, int y = 3)
        {
            base.Init(x, y);
            _objType = Define.ObjectType.Npc;
        }

        private void FixedUpdate()
        {
            KeyDown();
        }

        private void KeyDown()
        {
            
            if (_state == Define.ObjectState.KeyDown)
            {
                if (InputController.Instance.Interaction.Down)
                {
                    DoInteraction();
                    _state = Define.ObjectState.Ing;
                }
            }
        }
        public override void DoInteraction()
        {
            _ui = UIManager.Instance.ShowWorldSpaceUI<UI_TestConversation>();
            _ui.Owner = gameObject;
            (_ui as UI_TestConversation).Title = ScriptTitle;
            (_ui as UI_TestConversation).BagItem = BagItem;
            (_ui as UI_TestConversation).ClueItem = ClueItem;
            _ui.SetPosition(gameObject.transform.position, Vector3.up * 3);

            _player.GetComponent<PlayerCharacter>().Talk();

        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<PlayerCharacter>() == null) return;
            _state = Define.ObjectState.KeyDown;
            _player = collision.gameObject;
        }

        protected override void OnTriggerExit2D(Collider2D collision)
        {
            _state = Define.ObjectState.NonKeyDown;
            
            if (collision.gameObject.GetComponent<PlayerCharacter>() == null) return;
            if(_ui != null) _ui.CloseWorldSpaceUI();
            _player = null;
        }
    }
}