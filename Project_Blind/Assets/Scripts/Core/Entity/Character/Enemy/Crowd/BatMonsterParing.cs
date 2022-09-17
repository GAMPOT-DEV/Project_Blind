using UnityEngine;

namespace Blind
{
    public class BatMonsterParing: ParingEffect<BatMonster>
    {
        public override void OnCheckForParing(PlayerCharacter _player)
        {
            Debug.Log(_gameobject.name);
            if (_gameobject.isAttack())
            {
                _player.CharacterInvincible();
                if (_player.CurrentWaveGauge + _player.paringWaveGauge < _player.maxWaveGauge)
                    _player.CurrentWaveGauge += _player.paringWaveGauge;
                else
                    _player.CurrentWaveGauge = _player.maxWaveGauge;
            }
        }

        public override void EnemyDibuff()
        {
            if (_gameobject.CurrentStunGauge >= _gameobject.MaxStunGauge)
            {
                _gameobject.CoStun();
            }
            else
            {
                _gameobject.OnHurt();
            }
        }
    }
}