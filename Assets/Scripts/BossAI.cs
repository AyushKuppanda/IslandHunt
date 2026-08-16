using UnityEngine;

public class BossAI : MonoBehaviour
{
    public enum State { Idle, Chase, Melee, Fireball, Vulnerable }
    public State currentState = State.Idle;

    [Header("References")]
    public Transform player;
    public GameObject fireballPrefab;
    public Transform fireballSpawnPoint;

    [Header("Ranges")]
    public float detectionRange = 6f;
    public float meleeRange = 1.5f;

    [Header("Combat Pattern")]
    public int meleeAttacksBeforeFireball = 3;
    public float vulnerableDuration = 5f;
    public float attackCooldown = 1.2f;

    int meleeAttackCount = 0;
    float lastAttackTime = -999f;
    float vulnerableTimer = 0f;

    Animator animator;
    Rigidbody2D rb;
    EnemyHealth health;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<EnemyHealth>();

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
                if (distance <= meleeRange)
                {
                    currentState = State.Melee;
                }
                break;

            case State.Melee:
                animator.SetBool("IsMoving", false);

                if (distance > meleeRange)
                {
                    currentState = State.Chase;
                    break;
                }

                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    PerformMeleeAttack();
                }
                break;

            case State.Fireball:
                // handled via coroutine/trigger, see ShootFireball()
                break;

            case State.Vulnerable:
                vulnerableTimer -= Time.deltaTime;
                if (vulnerableTimer <= 0f)
                {
                    meleeAttackCount = 0;
                    currentState = State.Chase;
                }
                break;
        }
    }

    void FixedUpdate()
    {
        if (currentState == State.Chase)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * 1.5f; // boss move speed
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void FacePlayer()
    {
        float direction = player.position.x - transform.position.x;
        if (direction > 0) transform.localScale = new Vector3(1f, 1f, 1f);
        else if (direction < 0) transform.localScale = new Vector3(-1f, 1f, 1f);
    }

    void PerformMeleeAttack()
    {
        lastAttackTime = Time.time;
        meleeAttackCount++;

        int attackChoice = Random.Range(0, 2);
        animator.SetInteger("AttackIndex", attackChoice);
        animator.SetTrigger("AttackTrigger");

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(15);
        }

        if (meleeAttackCount >= meleeAttacksBeforeFireball)
        {
            ShootFireball();
        }
    }

    void ShootFireball()
    {
        currentState = State.Fireball;
        animator.SetTrigger("FireballTrigger"); // plays Idle animation (see Animator note above)

        Vector2 direction = (player.position - fireballSpawnPoint.position).normalized;
        GameObject fb = Instantiate(fireballPrefab, fireballSpawnPoint.position, Quaternion.identity);
        fb.GetComponent<Fireball>().SetDirection(direction);

        // After firing, become vulnerable
        vulnerableTimer = vulnerableDuration;
        currentState = State.Vulnerable;
    }

    public void Heal(int amount)
    {
        if (health != null)
        {
            health.currentHealth = Mathf.Clamp(health.currentHealth + amount, 0, health.maxHealth);
            if (health.healthBar != null)
            {
                health.healthBar.value = health.currentHealth;
            }
        }
    }
}
