using UnityEngine;

public abstract class Fighter : MonoBehaviour
{
    [Header("Character Stats")]
    [SerializeField] protected float moveSpeed = 10f;
    [SerializeField] public int Health = 100;
    [SerializeField] public int damage = 100;

    [Header("Character Status")]
    [SerializeField] protected bool isRunning;
    [SerializeField] protected bool isDie;
    [SerializeField] protected bool isFighting;
    public virtual void TakeDamge(int damage) { }
    public virtual void BeingHit() { }
    public virtual void Winner() { }
    public int SetHealth(int NewHealth)
    {
        Health = NewHealth;
        return Health;
    }
    public int SetDamage(int NewDamage)
    {
        damage = NewDamage;
        return damage;
    }
    protected virtual void Die() { }
    protected virtual void Attack() { }

}
