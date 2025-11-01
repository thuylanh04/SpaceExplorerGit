using UnityEngine;
using System.Collections.Generic;

public class AsteroidSpawner : MonoBehaviour
{
    [Header("Asteroid Settings")]
    public GameObject asteroidPrefab;
    public int asteroidCount = 5;
    public float asteroidSpeed = 2f;
    public float spawnPadding = 0.5f;

    private List<Rigidbody2D> asteroids = new List<Rigidbody2D>();
    private float xLimit, yLimit;

    void Start()
    {
        Camera cam = Camera.main;
        yLimit = cam.orthographicSize;
        xLimit = yLimit * cam.aspect;

        for (int i = 0; i < asteroidCount; i++)
        {
            float x = Random.Range(-xLimit + spawnPadding, xLimit - spawnPadding);
            float y = Random.Range(-yLimit + spawnPadding, yLimit - spawnPadding);
            Vector2 pos = new Vector2(x, y);

            GameObject asteroid = Instantiate(asteroidPrefab, pos, Quaternion.identity);

            Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 randomDir = Random.insideUnitCircle.normalized;
                rb.linearVelocity = randomDir * asteroidSpeed;
                rb.gravityScale = 0;
                asteroids.Add(rb);
            }
        }
    }

    void Update()
    {
        foreach (Rigidbody2D rb in asteroids)
        {
            if (rb == null) continue;
            Vector2 pos = rb.position;

            // Nếu chạm biên → bật ngược lại
            if (pos.x > xLimit || pos.x < -xLimit)
            {
                rb.linearVelocity = new Vector2(-rb.linearVelocity.x, rb.linearVelocity.y);
                pos.x = Mathf.Clamp(pos.x, -xLimit, xLimit);
            }

            if (pos.y > yLimit || pos.y < -yLimit)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -rb.linearVelocity.y);
                pos.y = Mathf.Clamp(pos.y, -yLimit, yLimit);
            }

            rb.position = pos;
        }
    }
}
