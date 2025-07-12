using UnityEngine;

public interface IDamagable 
{
    public void TakeDamage(int damage);

    public bool IsAlive { get;}

    public void Die();
}
