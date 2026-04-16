using UnityEngine;
using UnityEngine.SceneManagement; // Цей рядок дозволяє перезавантажувати рівні

public class GameManager : MonoBehaviour
{
    void Update()
    {
        // Якщо гра зупинена (Time.timeScale == 0)
        if (Time.timeScale == 0)
        {
            // І якщо гравець натиснув клавішу R
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Повертаємо час у норму
        // Перезавантажуємо поточну сцену
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}