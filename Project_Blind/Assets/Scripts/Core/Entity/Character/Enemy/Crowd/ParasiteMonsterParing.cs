using UnityEngine;

namespace Blind
{
    public class ParasiteMonsterParing: ParingEffect
    {
        private ParasiteMonster _character;
        private void Awake()
        {
            _character = gameObject.GetComponent<ParasiteMonster>();
        }
        public override void GetParing()
        {
            var player = GameManager.Instance.Player;
            if (_character.IsAttack)
            {
                player.CharacterInvincible();
                player.CurrentWaveGauge += player.paringWaveGauge;
                player._source.GenerateImpulse();
                player.isParingCheck = true;
                SoundManager.Instance.Play("Player/패링2", Define.Sound.Effect);
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