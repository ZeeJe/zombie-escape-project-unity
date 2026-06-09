using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { MainMenu, Playing, Paused, GameOver }
    public GameState CurrentState { get; private set; } = GameState.MainMenu;

    [Header("Scene Names")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string gameSceneName = "SampleScene";

    [Header("Gameplay Settings")]
    [SerializeField] private float distancePerSecond = 5f;
    [SerializeField] private float scoreMultiplier = 10f;

    private const string HighScoreKey = "ZE_HighScore";

    // Події для зв'язку з UIManager та іншими скриптами
    public event Action<GameState> OnStateChanged;
    public event Action<int, int> OnScoreUpdated; // (очки, дистанція)
    public event Action<int> OnHighScoreChanged;

    private int score;
    private int highScore;
    private float distance;

    private void Awake()
    {
        // Класичний Сінглтон, щоб GameManager не видалявся між сценами
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Підписка на подію завантаження сцен в Unity
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    // Автоматично спрацьовує, коли БУДЬ-ЯКА сцена завершила завантаження
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == mainMenuSceneName)
        {
            ResetGameplayData();
            SetState(GameState.MainMenu);
        }
        else if (scene.name == gameSceneName)
        {
            ResetGameplayData();
            Time.timeScale = 1f; // Переконаємось, що час не на паузі
            SetState(GameState.Playing);
        }
    }

    private void Update()
    {
        if (CurrentState != GameState.Playing) return;

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
        SceneManager.LoadScene(gameSceneName);
    }

    public void PauseGame()
    {
        if (CurrentState != GameState.Playing) return;

        Time.timeScale = 0f;
        SetState(GameState.Paused);
    }

    public void ResumeGame()
    {
        if (CurrentState != GameState.Paused) return;

        Time.timeScale = 1f;
        SetState(GameState.Playing);
    }

    public void GameOver()
    {
        if (CurrentState == GameState.GameOver) return;

        Time.timeScale = 0f;
        SetState(GameState.GameOver);
        SaveHighScore();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    private void ResetGameplayData()
    {
        score = 0;
        distance = 0f;
        OnScoreUpdated?.Invoke(0, 0); // Обнуляємо UI при старті
    }

    private void SetState(GameState newState)
    {
        CurrentState = newState;
        OnStateChanged?.Invoke(newState);
    }

    private void SaveHighScore()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(HighScoreKey, highScore);
            PlayerPrefs.Save();
            OnHighScoreChanged?.Invoke(highScore);
        }
    }

    // Геттери для безпечного читання даних іншими скриптами
    public int GetScore() => score;
    public int GetDistance() => Mathf.FloorToInt(distance);
    public int GetHighScore() => highScore;
}