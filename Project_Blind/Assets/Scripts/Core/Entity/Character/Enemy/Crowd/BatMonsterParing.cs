using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Blind
{
    public class BatMonsterParing: ParingEffect<BatMonster>
    {
        private float currentLens;
        private float CheckLens;
        private CinemachineVirtualCamera _camera;
        public override void OnCheckForParing(PlayerCharacter _player)
        {
            if (_gameobject.IsAttack)
            {
                SoundManager.Instance.Play("Player/패링2", Define.Sound.Effect);
                _player.CharacterInvincible();
                if (_player.CurrentWaveGauge + _player.paringWaveGauge < _player.maxWaveGauge)
                    _player.CurrentWaveGauge += _player.paringWaveGauge;
                else
                    _player.CurrentWaveGauge = _player.maxWaveGauge;
                _player._source.GenerateImpulse();
                _player.isParingCheck = true;
                EnemyDibuff();
            }
        }

        public override void EnemyDibuff()
        {
            _gameobject._attack.DisableDamage();
            _gameobject.OnStun();
        }

        
    }
}