using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject obstaclePrefab; // Сюди ми покладемо наш шаблон
    public float spawnRate = 2f;      // Час між появою нових перешкод (2 секунди)
    private float timer = 0f;

    void Update()
    {
        // Таймер, який рахує час
        if (timer < spawnRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            // Створюємо нову перешкоду на місці Спавнера
            Instantiate(obstaclePrefab, transform.position, transform.rotation);
            timer = 0f; // Скидаємо таймер
        }
    }
}