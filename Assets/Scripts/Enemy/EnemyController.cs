using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyController : Fighter
{
    private Animator animator;
    private NavMeshAgent agent;
    [SerializeField] private Transform Player;
    [SerializeField] private float AttackRange = 0.5f;
    [SerializeField] private float nextAttackTime = 1.5f;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;

    }

    void Update()
    {
        FollowPlayer();
        Attack();
    }

    void OnEnable()
    {
        GameManager.Instance.RegisterFighter(this);
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UnregisterFighter(this);
        }
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
        if (distanceToPlayer <= AttackRange)
        {
            StartCoroutine(WaitForNextAttack());
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
        GameManager.Instance.OnFighterDeath(this);
    }
    public override void Winner()
    {
        animator.SetTrigger("Win");
    }
    public override void BeingHit()
    {
        animator.SetTrigger("BeingHit");
        TakeDamge(damage);
    }
    IEnumerator WaitForNextAttack()
    {
        yield return new WaitForSeconds(nextAttackTime);
        animator.SetTrigger("Attack");
    }
}
