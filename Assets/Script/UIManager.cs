using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject hudPanel;
    public GameObject gameOverPanel;

    [Header("HUD")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI distanceText;

    [Header("Game Over")]
    public TextMeshProUGUI gameOverScoreText;
    public TextMeshProUGUI gameOverDistanceText;
    public TextMeshProUGUI highScoreText;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStateChanged += HandleStateChanged;
            GameManager.Instance.OnScoreUpdated += UpdateHUD;
            GameManager.Instance.OnHighScoreChanged += UpdateHighScore;
            HandleStateChanged(GameManager.Instance.CurrentState);
            UpdateHUD(GameManager.Instance.GetScore(), GameManager.Instance.GetDistance());
            UpdateHighScore(GameManager.Instance.GetHighScore());
        }
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStateChanged -= HandleStateChanged;
            GameManager.Instance.OnScoreUpdated -= UpdateHUD;
            GameManager.Instance.OnHighScoreChanged -= UpdateHighScore;
        }
    }

    void HandleStateChanged(GameManager.GameState state)
    {
        if (mainMenuPanel)
            mainMenuPanel.SetActive(state == GameManager.GameState.MainMenu);

        if (hudPanel)
            hudPanel.SetActive(state == GameManager.GameState.Playing || state == GameManager.GameState.Paused);

        if (gameOverPanel)
            gameOverPanel.SetActive(state == GameManager.GameState.GameOver);

        if (state == GameManager.GameState.GameOver)
            ShowGameOverStats();
    }

    void UpdateHUD(int score, int distance)
    {
        if (scoreText)
            scoreText.text = score.ToString();

        if (distanceText)
            distanceText.text = distance + " м";
    }

    void UpdateHighScore(int highScore)
    {
        if (highScoreText)
            highScoreText.text = "Рекорд: " + highScore;
    }

    void ShowGameOverStats()
    {
        if (gameOverScoreText)
            gameOverScoreText.text = "Рахунок: " + GameManager.Instance.GetScore();

        if (gameOverDistanceText)
            gameOverDistanceText.text = "Дистанція: " + GameManager.Instance.GetDistance() + " м";

        if (highScoreText)
            highScoreText.text = "Рекорд: " + GameManager.Instance.GetHighScore();
    }

    public void OnStartButton()
    {
        GameManager.Instance?.StartGame();
    }

    public void OnRestartButton()
    {
        GameManager.Instance?.RestartGame();
    }

    public void OnExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
