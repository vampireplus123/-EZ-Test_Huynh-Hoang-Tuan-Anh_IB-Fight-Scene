using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyController : Fighter
{
    private Animator animator;
    private NavMeshAgent agent;
    [SerializeField] private Transform Player;
    [SerializeField] private float AttackRange = 0.5f;
    [SerializeField] private float nextAttackTime = 0f;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        FollowPlayer();
        Attack();
    }


    void FollowPlayer()
    {
        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed);
        agent.SetDestination(Player.position);
    }

    protected override void Attack()
    {
        Vector3 EnemyTransform = this.transform.position;
        float distanceToPlayer = Vector3.Distance(EnemyTransform, Player.position);
        if (distanceToPlayer <= AttackRange&& Time.time >= nextAttackTime)
        {
            animator.SetTrigger("Attack");
        }

    }
    public override void TakeDamge(int damage)
    {
        if (Health <= 0)
        {
            Debug.Log("Over!!!");
            Die();
            return;
        }
        Health -= damage;
    }

    protected override void Die()
    {
        animator.SetTrigger("Die");
        agent.enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
    }
    public override void BeingHit()
    {
        animator.SetTrigger("BeingHit");
        TakeDamge(damage);
    }
}
