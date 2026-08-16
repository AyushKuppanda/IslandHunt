using UnityEngine;
using UnityEngine.SceneManagement;

public class BossDoorTrigger : MonoBehaviour
{
    public string bossSceneName; // exact name of your boss fight scene

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(bossSceneName);
        }
    }
}