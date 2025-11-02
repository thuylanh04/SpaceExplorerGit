using UnityEngine;
using System.Collections.Generic;

public class AsteroidSpawner : MonoBehaviour
{
    [Header("🪨 Asteroid Settings")]
    public GameObject asteroidPrefab;
    public int asteroidCount = 5;
    public float asteroidSpeed = 2f;
    public float spawnPadding = 0.5f;

    private readonly List<Rigidbody2D> asteroids = new List<Rigidbody2D>();
    private float xLimit, yLimit;

    void Start()
    {
        // ✅ Lấy kích thước vùng hiển thị dựa vào camera
        Camera cam = Camera.main;
        yLimit = cam.orthographicSize;
        xLimit = yLimit * cam.aspect;

        // ✅ Sinh asteroid ngẫu nhiên trong vùng màn hình
        for (int i = 0; i < asteroidCount; i++)
        {
            float x = Random.Range(-xLimit + spawnPadding, xLimit - spawnPadding);
            float y = Random.Range(-yLimit + spawnPadding, yLimit - spawnPadding);
            Vector2 pos = new Vector2(x, y);

            GameObject asteroid = Instantiate(asteroidPrefab, pos, Quaternion.identity);

            // ✅ Thêm Rigidbody2D nếu prefab chưa có
            Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
            if (rb == null) rb = asteroid.AddComponent<Rigidbody2D>();

            rb.gravityScale = 0;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            // ✅ Hướng ngẫu nhiên và vận tốc ban đầu
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            rb.linearVelocity = randomDir * asteroidSpeed;

            asteroids.Add(rb);
        }
    }

    void Update()
    {
        // ✅ Duyệt qua danh sách asteroid đang tồn tại
        for (int i = asteroids.Count - 1; i >= 0; i--)
        {
            Rigidbody2D rb = asteroids[i];
            if (rb == null)
            {
                asteroids.RemoveAt(i);
                continue;
            }

            Vector2 pos = rb.position;

            // ✅ Khi chạm biên màn hình thì bật ngược hướng
            if (pos.x > xLimit || pos.x < -xLimit)
            {
                rb.linearVelocity = new Vector2(-rb.linearVelocity.x, rb.linearVelocity.y);
                pos.x = Mathf.Clamp(pos.x, -xLimit + 0.1f, xLimit - 0.1f);
            }

            if (pos.y > yLimit || pos.y < -yLimit)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -rb.linearVelocity.y);
                pos.y = Mathf.Clamp(pos.y, -yLimit + 0.1f, yLimit - 0.1f);
            }

            rb.position = pos;
        }
    }
}
