using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 15;
    Vector2 direction;

    void Start()
    {
        Destroy(gameObject, 3f); // auto-despawn after 3 seconds
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Arrow hit: " + other.gameObject.name + " | Tag: " + other.tag);

        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}