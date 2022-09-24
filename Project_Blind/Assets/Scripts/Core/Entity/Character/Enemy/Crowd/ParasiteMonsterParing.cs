using UnityEngine;

namespace Blind
{
    public class ParasiteMonsterParing: ParingEffect<ParasiteMonster>
    {
        public override void OnCheckForParing(PlayerCharacter _player)
        {
            Debug.Log(_gameobject.name + " " + _gameobject.IsAttack);
            if (_gameobject.IsAttack)
            {
                _player.CharacterInvincible();
                if (_player.CurrentWaveGauge + _player.paringWaveGauge < _player.maxWaveGauge)
                    _player.CurrentWaveGauge += _player.paringWaveGauge;
                else
                    _player.CurrentWaveGauge = _player.maxWaveGauge;
                _player._source.GenerateImpulse();
                _player.isParingCheck = true;
                SoundManager.Instance.Play("Player/패링1", Define.Sound.Effect);
                EnemyDibuff();
            }
        }

        public override void EnemyDibuff()
        {
            _gameobject._attack.DisableDamage();
            if (_gameobject.CurrentStunGauge >= _gameobject.MaxStunGauge)
            {
                _gameobject.CoStun();
            }
            else
            {
                //_gameobject.OnHurt();
            }
        }
    }
}