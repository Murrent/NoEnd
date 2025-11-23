namespace Interfaces
{
    public interface IDamageable
    {
        bool dead {  get; }
        void TakeDamage(int damage);
    }
}