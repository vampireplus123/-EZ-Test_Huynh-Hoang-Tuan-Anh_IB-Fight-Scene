using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInput))]
public class Player : Fighter
{
    private Animator animator;
    private CharacterController characterController;
    private PlayerInput playerInput;
    private float movementX;
    private float movementY;

    [SerializeField] bool isRunning;
    [SerializeField] bool isFighting;
    [SerializeField] bool isDie;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        Vector3 inputDir = new Vector3(movementX, 0f, movementY);

        if (inputDir.magnitude > 0.1f)
        {
            // Tính góc hướng nhập vào
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

            // Di chuyển theo hướng nhân vật đã xoay
            Vector3 moveDir = transform.forward; // luôn đi tới theo mặt của nhân vật
            characterController.Move(moveDir * moveSpeed * Time.deltaTime);
        }
    }


    void OnMove(InputValue value)
    {
        Vector2 movementVector = value.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
        isRunning = true;
        isFighting = false;
        isDie = false;
    }
}
