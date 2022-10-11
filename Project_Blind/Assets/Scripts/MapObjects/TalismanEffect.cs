using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class TalismanEffect : MonoBehaviour
    {
        PlayerCharacter Player;

        public TalismanEffect()
        {
            Player = FindObjectOfType<PlayerCharacter>();
        }

        public void EquipTalisman(int itemId)
        {
            if(Player == null)
                Player = FindObjectOfType<PlayerCharacter>();

            switch (itemId)
            {
                case 1:
                    EquipBlue();
                    break;
                case 2:
                    EquipGreen();
                    break;
                case 3:
                    EquipPurple();
                    break;
                case 4:
                    EquipRed();
                    break;
                case 5:
                    EquipYellow();
                    break;
            }
        }

        public void UnequipTalisman(int itemId)
        {
            if (Player == null)
                Player = FindObjectOfType<PlayerCharacter>();

            switch (itemId)
            {
                case 1:
                    UnequipBlue();
                    break;
                case 2:
                    UnequipGreen();
                    break;
                case 3:
                    UnequipPurple();
                    break;
                case 4:
                    UnequipRed();
                    break;
                case 5:
                    UnequipYellow();
                    break;
            }
        }

        //Green: HP +
        private void EquipGreen()
        {
            Player.ChangeHp(3);
        }

        private void UnequipGreen()
        {
            Player.ChangeHp(-3);
        }

        //Red: Attack Damage +
        private void EquipRed()
        {
            Player.ChangeDamage(1);
        }

        private void UnequipRed()
        {
            Player.ChangeDamage(-1);
        }

        //Purple: Wave Gauge +
        private void EquipPurple()
        {
            Player.ChangeWaveGauge(1);
        }

        private void UnequipPurple()
        {
            Player.ChangeWaveGauge(-1);
        }

        //Blue: Speed +
        private void EquipBlue()
        {
            Player.ChangeSpeed(1);
        }

        private void UnequipBlue()
        {
            Player.ChangeSpeed(-1);
        }

        //Yellow: Coin +
        private void EquipYellow()
        {
            Player.ChangeMoneyProb(true);
        }

        private void UnequipYellow()
        {
            Player.ChangeMoneyProb(false);
        }
    }
}
