// Player.cs
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Player : Fighter
{
    private Vector2 movementVector;
    private Vector3 moveDirection;
    private Animator animator;
    private Rigidbody rb;
    private InputSystem_Actions inputActions;
    private float movementX;
    private float movementY;

    [SerializeField] private float rotationSpeed = 10f;

    [Header("Player Combo System")]
    [SerializeField] private int comboStep = 1;
    [SerializeField] private float comboTime = 0.5f;
    private float lastComboTime;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.Move.performed += context =>
        {
            if (!isAttacking) // Không cho di chuyển khi đang đánh
            {
                movementVector = context.ReadValue<Vector2>();
                movementX = movementVector.x;
                movementY = movementVector.y;
                moveDirection = new Vector3(movementX, 0f, movementY).normalized;
                isMoving = moveDirection.magnitude > 0.1f;
            }
        };

        inputActions.Player.Move.canceled += context =>
        {
            movementVector = Vector2.zero;
            moveDirection = Vector3.zero;
            isMoving = false;
        };

        inputActions.Player.Attack.performed += ctx => Attack();
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        GameManager.Instance.RegisterFighter(this);
        LevelManager.Instance.RegisterFighter(this);
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UnregisterFighter(this);
        }
        LevelManager.Instance.UnregisterFighter(this);
    }

    void Update()
    {
        if (CanMove())
        {
            Moving();
            Rotating();
            isMoving = moveDirection.magnitude > 0.1f;
        }
        else
        {
            animator.SetFloat("Speed", 0f);
            isMoving = false;
        }
    }

    protected override void Attack()
    {
        if (!CanAttack()) return;

        if (Time.time > lastComboTime + comboTime)
        {
            comboStep = 1;
        }
        else
        {
            comboStep++;
            if (comboStep > 2) comboStep = 1;
        }

        isAttacking = true;
        ComboAnimationControl(comboStep);
        isFighting = true;
        lastComboTime = Time.time;
        StartCoroutine(AfterAttackRoutine());
    }

    IEnumerator AfterAttackRoutine()
    {
        yield return new WaitForSeconds(1f);
        isFighting = false;
        isAttacking = false;
    }

    void ComboAnimationControl(int comboAnimation)
    {
        switch (comboAnimation)
        {
            case 1:
                animator.SetTrigger("PunchR");
                break;
            case 2:
                animator.SetTrigger("PunchL");
                break;
        }
    }

    void Moving()
    {
        Vector3 velocity = moveDirection * moveSpeed;
        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
        animator.SetFloat("Speed", moveDirection.magnitude);
    }

    void Rotating()
    {
        if (moveDirection.magnitude >= 0.1f)
        {
            
            if (isWinning)
            {
                isWinning = false;
                animator.SetTrigger("isIdle"); 
            }

            
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);

            animator.SetFloat("Speed", moveDirection.magnitude);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }

    }

    protected override void Die()
    {
        if (isDie || isMoving || isAttacking || isWinning) return;
        isDie = true;
        StartCoroutine(DieCoroutine());
    }

    public override void TakeDamge(int damage)
    {
        if (isDie || isMoving || isAttacking || isWinning) return;

        Health -= damage;
        if (Health <= 0)
        {
            Die();
            Debug.Log("Over!!!");
            return;
        }
    }

    public override void BeingHit()
    {
        Debug.Log("Enemy Hit toi");
        animator.SetTrigger("BeingHit");
        Debug.Log(Health);
    }

    public override void Winner()
    {
        if (!CanWin()) return;
        isWinning = true;
        animator.SetTrigger("Win");
    }

    IEnumerator DieCoroutine()
    {
        animator.SetTrigger("isKnockedOut");
        yield return new WaitForSeconds(2f);
        GetComponent<CapsuleCollider>().enabled = false;
        enabled = false;
        GameManager.Instance.OnFighterDeath(this);
    }
}
