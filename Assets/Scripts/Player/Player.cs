using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class Player : Fighter
{
    private Vector2 movementVector;
    private Vector3 MoveDirection;
    private Animator animator;
    private CharacterController characterController;
    private InputSystem_Actions inputActions;
    private float movementX;
    private float movementY;

    [Header("Player Combo System")]
    [SerializeField] private int comboStep = 1;
    [SerializeField] private float comboTime = 0.5f;
    private float lastComboTime;

    void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        inputActions = new InputSystem_Actions();

        inputActions.Player.Move.performed += ctx =>
        {
            movementVector = ctx.ReadValue<Vector2>();
            movementX = movementVector.x;
            movementY = movementVector.y;
            MoveDirection = new Vector3(movementX, 0f, movementY);
        };

        inputActions.Player.Move.canceled += ctx =>
        {
            movementVector = Vector2.zero;
            MoveDirection = Vector3.zero;
        };

        inputActions.Player.Attack.performed += ctx => Attack();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void Update()
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
        characterController.Move(MoveDirection * moveSpeed * Time.deltaTime);
        animator.SetFloat("Speed", MoveDirection.magnitude);
    }

    void Rotating()
    {
        if (MoveDirection.magnitude >= 0.1f)
        {
            float targetAngle = MathF.Atan2(MoveDirection.x, MoveDirection.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
            Moving();
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }
    }

    void PlayerControl()
    {
        Rotating();
    }
}
