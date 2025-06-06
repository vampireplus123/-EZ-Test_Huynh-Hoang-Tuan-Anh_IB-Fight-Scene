using UnityEngine;

public abstract class Fighter : MonoBehaviour
{
    [Header("Character Stats")]
    [SerializeField] protected float moveSpeed = 10f;
    [SerializeField] protected int Health = 100;
    [SerializeField] protected int damage = 100;

    [Header("Character Status")]
    [SerializeField] protected bool isRunning;
    [SerializeField] protected bool isDie;
    [SerializeField] protected bool isFighting;
    public virtual void TakeDamge(int damage) { }
    public virtual void BeingHit() { }
    public virtual void Winner() { }
    public virtual int SetHealth(int NewHealth)
    {
        Health = NewHealth;
        return Health;
    }
    public virtual int SetDamage(int NewDamage)
    {
        damage = NewDamage;
        Debug.Log($"{this.name} - New Damage: {damage}");
        return damage;
    }

    public virtual int GetHealth()
    {
        return Health;
    }
    public virtual int GetDamage()
    {
        return damage;
    }
    protected virtual void Die() { }
    protected virtual void Attack() { }

}
