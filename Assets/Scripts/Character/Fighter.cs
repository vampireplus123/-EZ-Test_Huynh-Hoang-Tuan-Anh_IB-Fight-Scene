using UnityEngine;

public abstract class Fighter : MonoBehaviour
{
    [Header("Character Stats")]
    [SerializeField] protected float moveSpeed = 10f;
    [SerializeField] protected int Health = 100;
    [SerializeField] protected int damage = 100;

    [Header("Character Status")]
    [SerializeField] protected bool isRunning;
    [SerializeField] public bool isDie;
    [SerializeField] protected bool isFighting;
    [SerializeField] protected bool isWinning;
    [SerializeField] protected bool isMoving;
    [SerializeField] protected bool isAttacking;

    public bool CanMove()
    {
        if (isDie)
            return false;
        if (isAttacking)
            return false;
        if (isWinning)
            return false;
        return true;
    }
    public bool CanAttack()
    {
        if (isDie)
            return false;
        if (isMoving)
            return false;
        if (isWinning)
            return false;
        return true;
    }

    public bool CanWin()
    {
        if (isDie)
            return false;
        if (isMoving)
            return false;
        if (isAttacking)
            return false;
        return true;
    }


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