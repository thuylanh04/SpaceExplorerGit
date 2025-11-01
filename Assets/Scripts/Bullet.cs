using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Bay thẳng theo trục Y dương (hướng lên)
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Asteroid"))
        {
            // Khi trúng thiên thạch: xóa cả hai và cộng điểm
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(5);
            }
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
