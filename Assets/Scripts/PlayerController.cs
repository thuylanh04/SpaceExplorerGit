using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Cài đặt di chuyển")]
    public float moveSpeed = 5f;     // tốc độ di chuyển của tàu

    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Nhận đầu vào từ bàn phím (WASD hoặc phím mũi tên)
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        // ✅ Di chuyển tàu vũ trụ
        rb.linearVelocity = moveInput * moveSpeed;

        // ✅ Giới hạn trong vùng hiển thị (để không bay ra ngoài màn hình)
        Vector2 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -8f, 8f);
        pos.y = Mathf.Clamp(pos.y, -4.5f, 4.5f);
        transform.position = pos;
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

        // ✅ Nếu va chạm với Asteroid → trừ điểm
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            Debug.Log("🚨 Va chạm thiên thạch - trừ điểm!");
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(-5);
            }
        }
    }
}
