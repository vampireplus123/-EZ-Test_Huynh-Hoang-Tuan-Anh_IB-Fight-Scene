using UnityEngine;

public abstract class Fighter : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 10f;
    [SerializeField] protected float Health = 100f;

    protected virtual void TakeDamge(int damage)
    {
        if (Health <= 0)
        {
            Debug.Log("Over!!!");
            return;
        }
        Health -= damage;
    }
    protected virtual void Die()
    {

    }

}
