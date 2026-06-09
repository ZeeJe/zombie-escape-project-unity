using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { MainMenu, Playing, Paused, GameOver }
    public GameState CurrentState { get; private set; } = GameState.MainMenu;

    [Header("Scene Settings")]
    [SerializeField] string mainMenuSceneName = "MainMenu";
    [SerializeField] string gameSceneName = "SampleScene";

    [Header("Gameplay")]
    [SerializeField] float distancePerSecond = 5f;
    [SerializeField] float scoreMultiplier = 10f;

    const string HighScoreKey = "ZE_HighScore";

    public event Action<GameState> OnStateChanged;
    public event Action<int, int> OnScoreUpdated;
    public event Action<int> OnHighScoreChanged;

    int score;
    int highScore;
    float distance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
    }

    void Start()
    {
        SetState(CurrentState);
    }

    void Update()
    {
        if (CurrentState != GameState.Playing)
            return;

        distance += Time.deltaTime * distancePerSecond;
        int currentDistance = Mathf.FloorToInt(distance);
        int currentScore = Mathf.FloorToInt(distance * scoreMultiplier);

        if (currentScore != score)
        {
            score = currentScore;
            OnScoreUpdated?.Invoke(score, currentDistance);
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        SetState(GameState.Playing);
        SceneManager.LoadScene(gameSceneName);
    }

    public void PauseGame()
    {
        if (CurrentState != GameState.Playing)
            return;

        Time.timeScale = 0f;
        SetState(GameState.Paused);
    }

    public void ResumeGame()
    {
        if (CurrentState != GameState.Paused)
            return;

        Time.timeScale = 1f;
        SetState(GameState.Playing);
    }

    public void GameOver()
    {
        if (CurrentState == GameState.GameOver)
            return;

        Time.timeScale = 0f;
        SetState(GameState.GameOver);
        SaveHighScore();
    }

    public void RestartGame()
    {
        score = 0;
        distance = 0f;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SetState(GameState.Playing);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
        SetState(GameState.MainMenu);
    }

    public int GetScore() => score;
    public int GetDistance() => Mathf.FloorToInt(distance);
    public int GetHighScore() => highScore;

    void SetState(GameState newState)
    {
        CurrentState = newState;
        OnStateChanged?.Invoke(newState);
    }

    void SaveHighScore()
    {
        if (score <= highScore)
            return;

        highScore = score;
        PlayerPrefs.SetInt(HighScoreKey, highScore);
        PlayerPrefs.Save();
        OnHighScoreChanged?.Invoke(highScore);
    }
}
