using UnityEngine;

public class ArcherAI : MonoBehaviour
{
    public enum State { Idle, Shoot }
    public State currentState = State.Idle;

    [Header("Detection")]
    public Transform player;
    public float shootRange = 5f;

    [Header("Shooting")]
    public GameObject arrowPrefab;
    public Transform firePoint;
    public float shootCooldown = 2f;
    float lastShootTime = -999f;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

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
                if (distance <= shootRange)
                {
                    currentState = State.Shoot;
                }
                break;

            case State.Shoot:
                if (distance > shootRange)
                {
                    currentState = State.Idle;
                }
                else if (Time.time >= lastShootTime + shootCooldown)
                {
                    Shoot();
                }
                break;
        }
    }

    void FacePlayer()
    {
        float direction = player.position.x - transform.position.x;

        if (direction > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f); // facing right
        }
        else if (direction < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f); // facing left
        }
    }

    void Shoot()
    {
        lastShootTime = Time.time;
        animator.SetTrigger("ShootTrigger");

        Vector2 shootDirection = (player.position - firePoint.position).normalized;

        GameObject arrowObj = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        Arrow arrowScript = arrowObj.GetComponent<Arrow>();
        arrowScript.SetDirection(shootDirection);
    }
}