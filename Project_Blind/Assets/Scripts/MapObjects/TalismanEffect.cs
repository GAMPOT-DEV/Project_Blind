using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class TalismanEffect : MonoBehaviour
    {
        PlayerCharacter Player;

        //Green: HP +
        public void EquipGreen()
        {
            Player.ChangeHp(3);
        }

        public void UnequipGreen()
        {
            Player.ChangeHp(-3);
        }

        //Red: Attack Damage +
        public void EquipRed()
        {
            Player.ChangeDamage(1);
        }

        public void UnequipRed()
        {
            Player.ChangeDamage(-1);
        }

        //Purple: Wave Gauge +
        public void EquipPurple()
        {
            Player.ChangeWaveGauge(1);
        }

        public void UnequipPurple()
        {
            Player.ChangeWaveGauge(-1);
        }

        //Blue: Speed +
        public void EquipBlue()
        {
            Player.ChangeWaveGauge(1);
        }

        public void UnequipBlue()
        {
            Player.ChangeWaveGauge(-1);
        }

        //Yellow: Coin +
        public void EquipYellow()
        {
            Player.ChangeMoneyProb(Player.MoneyDropProb);
        }

        public void UnequipYellow()
        {
            Player.ChangeMoneyProb(-Player.MoneyDropProb);
        }
    }
}
