using UnityEngine;
using System.Collections.Generic;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public int asteroidCount = 5;
    public float speed = 2f, padding = 0.5f;

    private List<Rigidbody2D> asteroids = new();
    private float xLimit, yLimit;

    void Start()
    {
        var cam = Camera.main;
        yLimit = cam.orthographicSize;  // Lấy chiều cao màn hình (orthographic)
        xLimit = yLimit * cam.aspect;   // Tính chiều rộng màn hình

        for (int i = 0; i < asteroidCount; i++)
        {
            Vector2 pos = new(Random.Range(-xLimit + padding, xLimit - padding),
                              Random.Range(-yLimit + padding, yLimit - padding));

            var asteroid = Instantiate(asteroidPrefab, pos, Quaternion.identity);
            var rb = asteroid.GetComponent<Rigidbody2D>() ?? asteroid.AddComponent<Rigidbody2D>();

            rb.gravityScale = 0;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
            rb.linearVelocity = Random.insideUnitCircle.normalized * speed;

            asteroids.Add(rb);
        }
    }

    void Update()
    {
        // Kiểm tra từng thiên thạch
        foreach (var rb in asteroids)
        {
            if (rb == null) continue; // Bỏ qua nếu đã bị hủy
            var p = rb.position; // Lấy vị trí hiện tại

            // Nếu chạm biên ngang
            if (Mathf.Abs(p.x) > xLimit)
            {
                // Đổi hướng vận tốc ngang
                rb.linearVelocity = new(-rb.linearVelocity.x, rb.linearVelocity.y);
                // Đẩy vào trong một chút để tránh kẹt biên
                p.x = Mathf.Sign(p.x) * (xLimit - 0.1f);
            }

            // Nếu chạm biên dọc
            if (Mathf.Abs(p.y) > yLimit)
            {
                // Đổi hướng vận tốc dọc
                rb.linearVelocity = new(rb.linearVelocity.x, -rb.linearVelocity.y);
                // Đẩy vào trong một chút
                p.y = Mathf.Sign(p.y) * (yLimit - 0.1f);
            }

            rb.position = p; // Cập nhật vị trí mới
        }
    }
}