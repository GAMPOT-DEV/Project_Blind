using UnityEngine;

namespace Blind
{
    public class BossHandParing: ParingEffect<BossHand>
    {
        public override void OnCheckForParing(PlayerCharacter _player)
        {
            _player.CharacterInvincible();
            _player.CurrentWaveGauge += _player.paringWaveGauge;
        }

        public override void EnemyDibuff()
        {
            _gameobject.Paring();
        }
    }
}