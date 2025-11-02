using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("🚀 Cài đặt di chuyển")]
    public float moveSpeed = 5f;

    [Header("🔫 Cài đặt bắn")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.25f;
    private float nextFireTime = 0f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Di chuyển
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        // Bắn laser
        if (Input.GetKey(KeyCode.Space) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void FixedUpdate()
    {
        // Di chuyển tàu
        rb.linearVelocity = moveInput * moveSpeed;

        // Giữ hướng cố định (luôn hướng lên)
        transform.rotation = Quaternion.identity;

        // Giới hạn tàu trong vùng màn hình
        Vector2 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -8f, 8f);
        pos.y = Mathf.Clamp(pos.y, -4.5f, 4.5f);
        transform.position = pos;
    }

    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ✅ Nếu va chạm với Star → cộng điểm
        if (collision.gameObject.CompareTag("Star"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(10);
            }
            Destroy(collision.gameObject);
        }

        // 🚨 Nếu va chạm với Asteroid → Game Over
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            Debug.Log("🚨 Va chạm thiên thạch - GAME OVER!");

            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
        }
    }
}
