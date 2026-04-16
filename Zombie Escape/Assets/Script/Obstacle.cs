using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        
        // Видаляємо квадрат, якщо він полетів далеко вліво, щоб не забивати пам'ять
        if (transform.position.x < -15) {
            Destroy(gameObject);
        }
    }

    // Цей метод спрацює, коли щось торкнеться нашого тригера
    void OnTriggerEnter2D(Collider2D other)
    {
        // Перевіряємо, чи це був об'єкт з тегом Player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Game Over!");
            Time.timeScale = 0; // Це повністю зупиняє час у грі
        }
    }
}