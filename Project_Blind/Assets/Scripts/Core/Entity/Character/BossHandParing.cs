using Cinemachine;
using UnityEngine;

namespace Blind
{
    public class BossHandParing: ParingEffect<BossHand>
    {
        public override bool OnCheckForParing(PlayerCharacter _player)
        {
            Debug.Log("실행됨");
            _player.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
            _player.CharacterInvincible();
            _player.CurrentWaveGauge += _player.paringWaveGauge;
            return true;
        }

        public override void EnemyDibuff()
        {
            _gameobject.Paring();
        }
    }
}