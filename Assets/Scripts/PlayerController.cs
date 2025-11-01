using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("🚀 Cài đặt di chuyển")]
    public float moveSpeed = 5f;     // tốc độ di chuyển của tàu

    [Header("🔫 Cài đặt bắn")]
    public GameObject bulletPrefab;  // prefab viên đạn
    public Transform firePoint;      // vị trí đầu mũi tàu
    public float fireRate = 0.25f;   // thời gian giữa các lần bắn
    private float nextFireTime = 0f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // ✅ Nhận đầu vào di chuyển (WASD hoặc phím mũi tên)
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        // ✅ Xử lý bắn laser (nhấn phím cách)
        if (Input.GetKey(KeyCode.Space) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void FixedUpdate()
    {
        // ✅ Di chuyển tàu
        rb.linearVelocity = moveInput * moveSpeed;

        // ✅ Giữ hướng cố định (luôn hướng lên)
        transform.rotation = Quaternion.identity;

        // ✅ Giới hạn trong vùng hiển thị (để không bay ra ngoài màn hình)
        Vector2 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -8f, 8f);
        pos.y = Mathf.Clamp(pos.y, -4.5f, 4.5f);
        transform.position = pos;
    }

    void Shoot()
    {
        // ✅ Tạo đạn tại vị trí FirePoint
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
                GameManager.Instance.AddScore(10);

            Destroy(collision.gameObject);
        }

        // ✅ Nếu va chạm với Asteroid → trừ điểm
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            Debug.Log("🚨 Va chạm thiên thạch - trừ điểm!");
            if (GameManager.Instance != null)
                GameManager.Instance.AddScore(-5);
        }
    }
}
