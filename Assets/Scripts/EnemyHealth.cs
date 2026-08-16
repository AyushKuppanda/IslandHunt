using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth;
    public Slider healthBar; // drag this enemy's health bar Slider here
    public bool isBoss = false; // check this ONLY on the Boss's EnemyHealth component
    public GameObject victoryScreen; // drag VictoryScreen here, only needed on the Boss

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        Debug.Log(gameObject.name + " took damage: " + amount + " | HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " died");

        if (isBoss && victoryScreen != null)
        {
            victoryScreen.SetActive(true);
        }

        if (EnemyCounter.Instance != null)
        {
            EnemyCounter.Instance.EnemyDefeated();
        }

        Destroy(gameObject);

    }
}