using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;

    Animator animator;
    Rigidbody2D rb;
    Vector2 moveInput;
    int currentDirection = 0; // 0=Front, 1=Left, 2=Right, 3=Up
    bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleMovementInput();
        HandleAttackInput();
    }

    void FixedUpdate()
    {
        if (!isAttacking)
        {
            rb.linearVelocity = moveInput * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero; // don't move while attacking
        }
    }

    void HandleMovementInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(horizontal, vertical).normalized;

        bool isMoving = moveInput.magnitude > 0.1f;
        animator.SetBool("IsMoving", isMoving);

        if (isMoving && !isAttacking)
        {
            // Determine facing direction based on strongest input axis
            if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
            {
                currentDirection = horizontal > 0 ? 2 : 1; // Right : Left
            }
            else
            {
                currentDirection = vertical > 0 ? 3 : 0; // Up : Front
            }

            animator.SetInteger("Direction", currentDirection);
        }
    }

    void HandleAttackInput()
    {
        if (isAttacking) return;

        if (Input.GetMouseButtonDown(0)) // Attack1
        {
            isAttacking = true;
            animator.SetInteger("AttackIndex", 0);
            animator.SetInteger("Direction", currentDirection);
            animator.SetTrigger("AttackTrigger");
            Invoke(nameof(EndAttack), 0.5f); // adjust to match your attack clip length
        }
        else if (Input.GetMouseButtonDown(1)) // Attack2
        {
            isAttacking = true;
            animator.SetInteger("AttackIndex", 1);
            animator.SetInteger("Direction", currentDirection);
            animator.SetTrigger("AttackTrigger");
            Invoke(nameof(EndAttack), 0.5f);
        }
    }

    void EndAttack()
    {
        isAttacking = false;
    }
}
