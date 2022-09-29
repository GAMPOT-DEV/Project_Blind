using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class SavePoint : InteractionAble
    {
        private PlayerCharacter Player;
        public void Heal() // 파동게이지 및 HP회복
        {
            Player.CurrentWaveGauge = Player.maxWaveGauge;
            Player.Hp.ResetHp();
        }

        public void ResetEnemy()
        {
            GameManager.Instance.ResetStage();
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
                    //특정 키 눌러 ui interact하면 진행
                    if(true)
                    {
                        DoInteraction();
                    }
                    
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
            Heal();
            ResetEnemy();
        }
    }

}
