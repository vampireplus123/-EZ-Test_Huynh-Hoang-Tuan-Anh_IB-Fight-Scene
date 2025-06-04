using UnityEngine;

public abstract class Fighter : MonoBehaviour
{
    [Header("Character Stats")]
    [SerializeField] protected float moveSpeed = 10f;
    [SerializeField] protected float Health = 100f;

    [Header("Character Status")]
    [SerializeField] protected bool isRunning;
    [SerializeField] protected bool isDie;
    [SerializeField] protected bool isFighting;
    protected virtual void TakeDamge(int damage)
    {
        if (Health <= 0)
        {
            Debug.Log("Over!!!");
            Die();
            return;
        }
        Health -= damage;
    }
    protected virtual void Die() { }
    protected virtual void Attack() { }
    

}
