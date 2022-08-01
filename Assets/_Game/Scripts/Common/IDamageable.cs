namespace Common
{
    public interface IDamageable
    {
        public DamageGroup DamageGroup { get; }


        public void ReceiveDamage(int damage);
    }
}