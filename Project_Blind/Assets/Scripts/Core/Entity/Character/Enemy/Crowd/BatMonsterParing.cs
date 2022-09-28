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
                _player.CharacterInvincible();
                if (_player.CurrentWaveGauge + _player.paringWaveGauge < _player.maxWaveGauge)
                    _player.CurrentWaveGauge += _player.paringWaveGauge;
                else
                    _player.CurrentWaveGauge = _player.maxWaveGauge;
                _player._source.GenerateImpulse();
                _player.isParingCheck = true;
                SoundManager.Instance.Play("Player/패링2", Define.Sound.Effect);
                EnemyDibuff();
            }
        }
        public IEnumerator CameraZoomIn()
        {
            float current = _camera.m_Lens.OrthographicSize;
            Debug.Log(current);
            _camera.m_Lens.OrthographicSize = current - 10f;
            yield return new WaitForSeconds(0.1f);
            Debug.Log(current);
            _camera.m_Lens.OrthographicSize = current;
        }

        public override void EnemyDibuff()
        {
            _gameobject._attack.DisableDamage();
            _gameobject.OnStun();
        }

        
    }
}