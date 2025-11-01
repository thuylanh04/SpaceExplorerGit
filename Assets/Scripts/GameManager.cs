using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;   // ✅ Singleton instance

    public int score = 0;
    public TMP_Text scoreText;


    private void Awake()
    {
        // Chỉ cho phép 1 GameManager tồn tại
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

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    public int GetScore()
    {
        return score;
    }

    public void GameOver()
    {
        SceneManager.LoadScene("EndGame");
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}
