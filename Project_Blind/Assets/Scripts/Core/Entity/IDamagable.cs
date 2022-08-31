namespace Blind
{
    public interface IDamagable
    {
        public UnitHP Hp { get; }
        public void Hitted(float damage);
    }
}