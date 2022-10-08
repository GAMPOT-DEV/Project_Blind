using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Blind
{
    public class BatMonsterParing: ParingEffect
    {
        private float currentLens;
        private float CheckLens;
        private CinemachineVirtualCamera _camera;
        private BatMonster _character;

        private void Awake()
        {
            _character = gameObject.GetComponent<BatMonster>();
        }

        public override void GetParing()
        {
            var player = GameManager.Instance.Player;
            if (_character.IsAttack)
            {
                SoundManager.Instance.Play("Player/패링2", Define.Sound.Effect);
                player.CharacterInvincible();
                player.CurrentWaveGauge += player.paringWaveGauge;
                player._source.GenerateImpulse();
                player.isParingCheck = true;
                EnemyDebuff();
            }
        }

        public override void EnemyDebuff()
        {
            _character._attack.DisableDamage();
            _character.OnStun();
        }
    }
}