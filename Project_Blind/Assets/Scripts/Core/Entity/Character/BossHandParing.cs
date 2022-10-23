using Cinemachine;
using UnityEngine;

namespace Blind
{
    public class BossHandParing: ParingEffect
    {
        public override void GetParing()
        {
            SoundManager.Instance.Play("Player/패링2", Define.Sound.Effect);
            var player = GameManager.Instance.Player;
            player._source.GenerateImpulse();
            player.CharacterInvincible();
            player.CurrentWaveGauge += player.paringWaveGauge;
            EnemyDebuff();
        }

        public override void EnemyDebuff()
        {
            gameObject.GetComponent<BossHand>().Paring();
        }
    }
}