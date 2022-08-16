using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class InteractionTest : InteractionAble
    {
        int _x = 7;
        int _y = 7;

        protected override void Init(int x = 5, int y = 5)
        {
            base.Init(_x, _y);
        }
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<PlayerCharacter>() == null) return;
            // UI_TestInteraction를 WorldSpace로 띄운다.

            UIManager.Instance.KeyInputEvents -= HandleKeyInput;
            UIManager.Instance.KeyInputEvents += HandleKeyInput;

            _ui = UIManager.Instance.ShowWorldSpaceUI<UI_TestInteraction>();
            _ui.SetPosition(gameObject.transform.position, Vector3.up * 3);
        }
        protected override void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<PlayerCharacter>() == null) return;

            UIManager.Instance.KeyInputEvents -= HandleKeyInput;

            if (_ui != null)
                _ui.CloseWorldSpaceUI();
        }
        private void HandleKeyInput()
        {
            if (InputController.Instance.Interaction.Down)
            {
                if (_ui != null)
                    _ui.CloseWorldSpaceUI();

                UIManager.Instance.KeyInputEvents -= HandleKeyInput;
            }
        }
        public override void DoInteraction()
        {
            
        }
    }
}

