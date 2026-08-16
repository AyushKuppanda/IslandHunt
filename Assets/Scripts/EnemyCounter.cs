using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    public static EnemyCounter Instance;

    public int totalEnemies;
    int enemiesDefeated = 0;

    public GameObject castleDoor; // door blocking entry until unlocked

    void Awake()
    {
        Instance = this;
    }

    public void EnemyDefeated()
    {
        enemiesDefeated++;
        Debug.Log("Enemies defeated: " + enemiesDefeated + " / " + totalEnemies);

        if (enemiesDefeated >= totalEnemies)
        {
            UnlockDoor();
        }
    }

    void UnlockDoor()
    {
        Debug.Log("All enemies defeated - door unlocked!");
        if (castleDoor != null)
        {
            castleDoor.SetActive(false); // disables the blocking door/collider
        }
    }
}