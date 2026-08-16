using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State { Idle, Chase, Attack }
    public State currentState = State.Idle;

    [Header("Detection")]
    public Transform player;
    public float detectionRange = 5f;
    public float attackRange = 1.2f;

    [Header("Movement")]
    public float moveSpeed = 2f;

    [Header("Attack")]
    public float attackCooldown = 1.5f;
    float lastAttackTime = -999f;

    Animator animator;
    Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (player == null)
        {
            GameObject found = GameObject.FindGameObjectWithTag("Player");
            if (found != null) player = found.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        FacePlayer();

        switch (currentState)
        {
            case State.Idle:
                animator.SetBool("IsMoving", false);
                if (distance <= detectionRange)
                {
                    currentState = State.Chase;
                }
                break;

            case State.Chase:
                animator.SetBool("IsMoving", true);

                if (distance <= attackRange)
                {
                    currentState = State.Attack;
                }
                else if (distance > detectionRange * 1.5f)
                {
                    currentState = State.Idle;
                }
                break;

            case State.Attack:
                animator.SetBool("IsMoving", false);

                if (distance > attackRange)
                {
                    currentState = State.Chase;
                }
                else if (Time.time >= lastAttackTime + attackCooldown)
                {
                    PerformAttack();
                }
                break;
        }
    }

    void FixedUpdate()
    {
        if (currentState == State.Chase)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void FacePlayer()
    {
        float direction = player.position.x - transform.position.x;

        if (direction > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (direction < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    void PerformAttack()
    {
        lastAttackTime = Time.time;

        int attackChoice = Random.Range(0, 2);
        animator.SetInteger("AttackIndex", attackChoice);
        animator.SetTrigger("AttackTrigger");

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10);
        }
    }
}