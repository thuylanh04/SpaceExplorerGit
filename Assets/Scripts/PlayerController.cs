using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject bulletPrefab; // Prefab viên đạn sẽ được tạo ra
    public Transform firePoint;
    public float fireRate = 0.25f;
    private float nextFireTime = 0f;
    // nextFireTime = 0f: Bắn được ngay từ đầu game
    // nextFireTime = 2f: Phải đợi 2 giây từ lúc bắt đầu game mới bắn được
    // Không khai báo giá trị: Tự động = 0f, vẫn bắn được ngay từ đầu

    private Rigidbody2D rb;
    private Vector2 moveInput;  // Vector lưu hướng di chuyển

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Space) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;

        // Giữ hướng cố định (luôn hướng lên)
        transform.rotation = Quaternion.identity;

        // Giới hạn tàu trong vùng màn hình
        Vector2 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -9f, 9f);
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

    private void OnCollisionEnter2D(Collision2D collision)  // Hàm xử lý va chạm
    {
        if (collision.gameObject.CompareTag("Star"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(5);
            }
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Asteroid"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
        }
    }
}
