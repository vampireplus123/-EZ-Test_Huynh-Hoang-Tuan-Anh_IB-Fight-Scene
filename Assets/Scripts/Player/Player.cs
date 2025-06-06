using System;
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
            movementVector = context.ReadValue<Vector2>();
            movementX = movementVector.x;
            movementY = movementVector.y;
            moveDirection = new Vector3(movementX, 0f, movementY).normalized;
        };

        inputActions.Player.Move.canceled += context =>
        {
            movementVector = Vector2.zero;
            moveDirection = Vector3.zero;
        };
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        GameManager.Instance.RegisterFighter(this);
        LevelManager.Instance.RegisterFighter(this);

        inputActions.Player.Attack.performed += ctx => Attack();
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

    void FixedUpdate()
    {
        PlayerControl();
    }

    protected override void Attack()
    {
        if (Time.time > lastComboTime + comboTime)
        {
            comboStep = 1;
        }
        else
        {
            comboStep++;
            if (comboStep > 2) comboStep = 1;
        }

        ComboAnimationControl(comboStep);
        isFighting = true;
        lastComboTime = Time.time;
        StartCoroutine(MoveWhenAfterCombo());
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
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);

            float rotationSpeed = 10f;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }
    }


    protected override void Die()
    {
        if (isDie) return;
        isDie = true;
        StartCoroutine(DieCoroutine());
    }
    public override void TakeDamge(int damage)
    {
        if (Health <= 0)
        {
            Die();
            Debug.Log("Over!!!");
            return;
        }
        Health -= damage;
    }
    public override void BeingHit()
    {
        animator.SetTrigger("BeingHit");
        Debug.Log("Enemy Hit toi");
        Debug.Log(Health);
    }

    public override void Winner()
    {
        animator.SetTrigger("Win");
    }
    void PlayerControl()
    {
        if (isFighting || isDie)
        {
            animator.SetFloat("Speed", 0f);
            return;
        }
        Rotating();
        Moving();
    }

    IEnumerator MoveWhenAfterCombo()
    {
        yield return new WaitForSeconds(1);
        isFighting = false;
    }
    IEnumerator DieCoroutine()
    {
        animator.SetTrigger("isKnockedOut");
        yield return new WaitForSeconds(2f);
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        gameObject.GetComponent<Player>().enabled = false;
        GameManager.Instance.OnFighterDeath(this);
    }

}
