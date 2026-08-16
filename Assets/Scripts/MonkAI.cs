using UnityEngine;

public class MonkAI : MonoBehaviour
{
    public BossAI boss; // drag the Boss GameObject here
    public float healInterval = 7f;
    public int healAmount = 30;

    Animator animator;
    float healTimer;

    void Start()
    {
        animator = GetComponent<Animator>();
        healTimer = healInterval;
    }

    void Update()
    {
        healTimer -= Time.deltaTime;

        if (healTimer <= 0f)
        {
            HealBoss();
            healTimer = healInterval;
        }
    }

    void HealBoss()
    {
        if (boss == null) return;

        animator.SetTrigger("HealTrigger");
        boss.Heal(50); // adjust heal amount as needed
    }
}
