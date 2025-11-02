using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score = 0;
    public TMP_Text scoreText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateScoreUI();
    }

    // ✅ Cộng hoặc trừ điểm
    public void AddScore(int amount)
{
    score += amount;
    UpdateScoreUI();

    // Trì hoãn kiểm tra EndGame để đảm bảo các object bị Destroy() đã biến mất
    CancelInvoke(nameof(CheckGameEnd));
    Invoke(nameof(CheckGameEnd), 0.2f);
}


    // ✅ Cập nhật UI hiển thị điểm
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
        else
        {
            // Khi load scene mới, tự động tìm lại ScoreText (tránh null)
            TMP_Text foundText = GameObject.Find("ScoreText")?.GetComponent<TMP_Text>();
            if (foundText != null)
            {
                scoreText = foundText;
                scoreText.text = "Score: " + score;
            }
        }
    }

    // ✅ Kiểm tra nếu người chơi đã thu thập và phá hết vật thể
    private void CheckGameEnd()
{
    int starCount = GameObject.FindGameObjectsWithTag("Star").Length;
    int asteroidCount = GameObject.FindGameObjectsWithTag("Asteroid").Length;

    Debug.Log($"🪐 Kiểm tra EndGame: Star còn lại = {starCount}, Asteroid còn lại = {asteroidCount}");

    // Log vị trí từng asteroid còn lại
    foreach (var a in GameObject.FindGameObjectsWithTag("Asteroid"))
    {
        Debug.Log($"➡️ Asteroid còn lại: {a.name} tại {a.transform.position}");
    }

    if (starCount == 0 && asteroidCount == 0)
    {
        Debug.Log("🎯 Hoàn thành trò chơi!");
        PlayerPrefs.SetInt("FinalScore", score);
        SceneManager.LoadScene("EndGame");
    }
}


    // ✅ Gọi khi va chạm thiên thạch hoặc thắng game
    public void EndGame()
    {
        PlayerPrefs.SetInt("FinalScore", score);
        SceneManager.LoadScene("EndGame");
    }

    // ✅ Getter cho điểm hiện tại
    public int GetScore()
    {
        return score;
    }

    // ✅ Khi va chạm thiên thạch → GameOver
    public void GameOver()
    {
        Debug.Log("💥 Game Over!");
        EndGame();
    }
}
