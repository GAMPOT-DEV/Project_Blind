namespace Blind
{
    public class ProjectileEffect : ParingEffect
    {
        public override void GetParing()
        {
                PlayerCharacter player = GameManager.Instance.Player;
                player.CharacterInvincible();
                player.CurrentWaveGauge += player.paringWaveGauge;
                player._source.GenerateImpulse();
                player.isParingCheck = true;
                SoundManager.Instance.Play("Player/패링1", Define.Sound.Effect);
                gameObject.GetComponent<Projectile>().OnParing();
        }
        public override void EnemyDebuff()
        {
        }
    }
}