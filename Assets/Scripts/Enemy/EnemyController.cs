using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyController : Fighter
{
    private Animator animator;
    private NavMeshAgent agent;
    [SerializeField] private Transform PlayerTransform;
    [SerializeField] private float AttackRange = 0.5f;
    [SerializeField] private float nextAttackTime = 1.5f;
    [SerializeField] private float spread = 0.5f;
    bool isClose;
    private float lastAttackTime = 0f;

    void OnEnable()
    {
        GameManager.Instance.RegisterFighter(this);
        LevelManager.Instance.RegisterFighter(this);
    }
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        GameObject PlayerDestination = GameObject.FindGameObjectWithTag("Player");
        PlayerTransform = PlayerDestination.transform;
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UnregisterFighter(this);
        }
    }
    void Update()
    {
        FollowPlayer();
        LookAtPlayer();
        Attack();
    }


    void FollowPlayer()
    {
        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed);

        Vector3 offset = new Vector3
        (
            Mathf.Sin(GetInstanceID() * 0.1f) * spread,
            0,
            Mathf.Cos(GetInstanceID() * 0.1f) * spread
        );

        Vector3 targetPosition = PlayerTransform.position + offset;
        agent.SetDestination(targetPosition);
    }

    void LookAtPlayer()
    {
        Vector3 lookDir = PlayerTransform.position - transform.position;
        lookDir.y = 0f;
        if (lookDir != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 5f);
        }
    }


    protected override void Attack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, PlayerTransform.position);
        if (distanceToPlayer <= AttackRange)
        {
            isClose = true;
            Debug.Log(isClose);
            if (Time.time >= lastAttackTime + nextAttackTime)
            {
                lastAttackTime = Time.time;
                animator.SetTrigger("Attack");
            }
        }
        else
        {
            isClose = false;
            Debug.Log(isClose);
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
    public override int SetDamage(int NewDamage)
    {
        return base.SetDamage(NewDamage);
    }
}
