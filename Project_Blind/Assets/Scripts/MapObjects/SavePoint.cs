using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class SavePoint : InteractionAble
    {
        private PlayerCharacter Player;
        public void HealWaveGauge() // 파동게이지 회복
        {
            Player.CurrentWaveGauge = Player.maxWaveGauge;
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Player = collision.GetComponent<PlayerCharacter>();
                if(Player != null)
                {
                    _ui = UIManager.Instance.ShowWorldSpaceUI<UI_TestInteraction>();
                    _ui.SetPosition(transform.position, Vector3.down * 3);
                    DoInteraction();
                }
                
            }
            
        }
        
        protected override void OnTriggerExit2D(Collider2D collision)
        {
            if (_ui != null)
                _ui.CloseWorldSpaceUI();
        }

        public override void DoInteraction()
        {
            HealWaveGauge();

        }
    }

}
