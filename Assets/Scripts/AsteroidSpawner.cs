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
        yLimit = cam.orthographicSize;
        xLimit = yLimit * cam.aspect;

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
        foreach (var rb in asteroids)
        {
            if (rb == null) continue;
            var p = rb.position;

            if (Mathf.Abs(p.x) > xLimit)
            {
                rb.linearVelocity = new(-rb.linearVelocity.x, rb.linearVelocity.y);
                p.x = Mathf.Sign(p.x) * (xLimit - 0.1f);
            }

            if (Mathf.Abs(p.y) > yLimit)
            {
                rb.linearVelocity = new(rb.linearVelocity.x, -rb.linearVelocity.y);
                p.y = Mathf.Sign(p.y) * (yLimit - 0.1f);
            }

            rb.position = p;
        }
    }
}
