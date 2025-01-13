using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlatformerMovementController : MovementController
{
    [Header("Configuração de Movimentação")]
    public List<KeyCode> rightKeys = new List<KeyCode> { KeyCode.D, KeyCode.RightArrow };
    public List<KeyCode> leftKeys = new List<KeyCode> { KeyCode.A, KeyCode.LeftArrow };
    public Joystick joystick;

    [Header("Configurações de Velocidade")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public KeyCode runKey = KeyCode.LeftShift;

    [Header("Estado do Jogador")]
    public bool isJumping = false; // Atualizado pelo script de pulo
    public bool isFalling = false; // Atualizado pelo script de pulo

    private InputHandler inputHandler;
    private Animator animator;
    private bool isFacingRight = true;

    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>() ?? gameObject.AddComponent<InputHandler>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector2 inputDirection = joystick != null
            ? inputHandler.GetJoystickInput(joystick)
            : inputHandler.GetKeyboardInput(rightKeys, leftKeys);

        float speed = Input.GetKey(runKey) ? runSpeed : walkSpeed;

        Move(inputDirection, speed);

        UpdateAnimator(inputDirection, speed);

        FlipSprite(inputDirection.x);
    }

    private void UpdateAnimator(Vector2 inputDirection, float speed)
    {
        animator.SetBool("IsMoving", inputDirection.magnitude > 0);
        animator.SetFloat("MoveSpeed", Mathf.Abs(speed));

        isJumping = animator.GetBool("IsJumping");
        isFalling = animator.GetBool("IsFalling");
    }

    private void FlipSprite(float horizontalInput)
    {
        if (horizontalInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && isFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
