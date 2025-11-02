using UnityEngine;

public class StarSpawner : MonoBehaviour
{
    public GameObject starPrefab;
    public int starCount = 10;
    public float xRange = 8f;   // Giới hạn ngang
    public float yRange = 4.5f; // Giới hạn dọc

    void Start()
    {
        for (int i = 0; i < starCount; i++)
        {
            // Tạo vị trí ngẫu nhiên trong vùng giới hạn màn hình
            Vector2 pos = new Vector2(
                Random.Range(-xRange, xRange),
                Random.Range(-yRange, yRange)
            );

            // Sinh ra starPrefab ở vị trí đó
            Instantiate(starPrefab, pos, Quaternion.identity);
        }
    }
}
