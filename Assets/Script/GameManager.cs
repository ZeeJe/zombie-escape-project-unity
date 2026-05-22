using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Цей рядок обов'язковий для роботи з TextMeshPro!

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Сюди ми покладемо наш текст з екрана
    private float score = 0f;         // Змінна для збереження рахунку

    void Update()
    {
        // Якщо гра йде (не на паузі)
        if (Time.timeScale > 0)
        {
            // Рахунок постійно росте з часом
            score += Time.deltaTime * 10; 
            
            // Оновлюємо текст на екрані (відкидаємо дробові числа за допомогою Mathf.FloorToInt)
            scoreText.text = "Рахунок: " + Mathf.FloorToInt(score).ToString();
        }
        else
        {
            // Якщо гра зупинена і натиснули R
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}