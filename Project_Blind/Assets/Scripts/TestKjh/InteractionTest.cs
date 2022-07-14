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

        private void FixedUpdate()
        {
            if (InputController.Instance.Interaction.Down)
            {
                _ui.CloseWorldSpaceUI();
            }
        }

        protected override void Init(int x = 5, int y = 5)
        {
            base.Init(_x, _y);
        }
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<PlayerCharacter>() == null) return;
            // UI_TestInteraction를 WorldSpace로 띄운다.
            _ui = UIManager.Instance.ShowWorldSpaceUI<UI_TestInteraction>();
            _ui.SetPosition(gameObject.transform.position, Vector3.up * 3);
        }
        protected override void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<PlayerCharacter>() == null) return;
            _ui.CloseWorldSpaceUI();
        }
        public override void DoInteraction()
        {
            
        }
    }
}

