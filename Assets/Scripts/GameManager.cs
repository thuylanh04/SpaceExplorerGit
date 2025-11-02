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

    // Cộng hoặc trừ điểm
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();

        // Trì hoãn kiểm tra EndGame để đảm bảo các object bị Destroy() đã biến mất
        CancelInvoke(nameof(CheckGameEnd));
        Invoke(nameof(CheckGameEnd), 0.2f);
    }


    // Cập nhật UI hiển thị điểm
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

    private void CheckGameEnd()
    {
        int starCount = GameObject.FindGameObjectsWithTag("Star").Length;
        int asteroidCount = GameObject.FindGameObjectsWithTag("Asteroid").Length;

        if (starCount == 0 && asteroidCount == 0)
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        PlayerPrefs.SetInt("FinalScore", score);
        SceneManager.LoadScene("EndGame");
    }

    public int GetScore()
    {
        return score;
    }

    public void GameOver()
    {
        EndGame();
    }
}
