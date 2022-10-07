using Cinemachine;
using UnityEngine;

namespace Blind
{
    public class BossHandParing: ParingEffect
    {
        public override void GetParing()
        {
            var player = GameManager.Instance.Player;
            player.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
            player.CharacterInvincible();
            player.CurrentWaveGauge += player.paringWaveGauge;
        }

        public override void EnemyDebuff()
        {
            gameObject.GetComponent<BossHand>().Paring();
        }
    }
}