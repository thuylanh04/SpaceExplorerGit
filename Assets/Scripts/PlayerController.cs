using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;

    [Header("Shooting")]
    public GameObject laserPrefab;  // Prefab laser
    public Transform firePoint;     // Fire point trên spaceship

    private float xMin, xMax, yMin, yMax;

    void Start()
    {
        // Tính giới hạn theo camera orthographic
        Camera cam = Camera.main;
        float halfWidth = cam.orthographicSize * cam.aspect;
        float halfHeight = cam.orthographicSize;

        // Giới hạn x, y dựa trên camera
        xMin = -halfWidth;
        xMax = halfWidth;
        yMin = -halfHeight;
        yMax = halfHeight;
    }

    void Update()
    {
        // --- Movement ---
        float moveX = Input.GetAxis("Horizontal"); // Arrow keys / A,D
        float moveY = Input.GetAxis("Vertical");   // Arrow keys / W,S

        Vector3 pos = transform.position + new Vector3(moveX, moveY, 0) * speed * Time.deltaTime;

        // Clamp để giữ Player trong màn hình
        pos.x = Mathf.Clamp(pos.x, xMin, xMax);
        pos.y = Mathf.Clamp(pos.y, yMin, yMax);

        transform.position = pos;

        // --- Shooting ---
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (laserPrefab != null && firePoint != null)
                Instantiate(laserPrefab, firePoint.position, firePoint.rotation);
        }
    }
}
