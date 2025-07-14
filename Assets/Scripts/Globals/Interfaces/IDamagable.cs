/// <summary>
/// Interface for objects that can be damaged.
/// </summary>
public interface IDamagable 
{
    public void TakeDamage(int damage);

    public bool IsAlive { get;}

    public void Die();
}
